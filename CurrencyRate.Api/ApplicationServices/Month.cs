using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CurrencyRate.Api.ApplicationServices
{

    public class Week
    {
        const string DECIMAL_FORMAT = "0.00";
        private readonly List<Rate> _rates;

        decimal GetMediana(decimal[] values)
        {
            var r = values.Length % 2;
            if (r == 0)
            {
                var i2 = values.Length / 2;
                var i1 = i2 - 1;

                return (values[i1] + values[i2]) / 2;
            }

            return values[values.Length / 2];
        }

        (string Code, decimal[] Values)[] GroupRatesByCode()
        {

            return Rates
                .GroupBy(x => x.Code)
                .Select(g =>
                (
                    Code: g.Key,
                    Values: g.Select(rate => rate.Value / rate.Amount).OrderBy(val => val).ToArray()
                ))
                .ToArray();
        }

        public Week(DateTime startedOn)
        {
            _rates = new List<Rate>();
            StartedOn = startedOn;
            
            //если месяц начинается в СБ или ВС
            if (startedOn.DayOfWeek == DayOfWeek.Saturday)
            {
                StartedOn = StartedOn.AddDays(2);
            }
            else if(startedOn.DayOfWeek == DayOfWeek.Sunday)
            {
                StartedOn = StartedOn.AddDays(1);
            }

            
            var daysInMonth = DateTime.DaysInMonth(StartedOn.Year, StartedOn.Month);


            int diff = 5 - (int)StartedOn.DayOfWeek;

            if (diff >= 0)
            {
                int lastDayOfWeekNumber = StartedOn.Day + diff;
                lastDayOfWeekNumber = lastDayOfWeekNumber > daysInMonth ? daysInMonth : lastDayOfWeekNumber;
                FinishedOn = new DateTime(StartedOn.Year, StartedOn.Month, lastDayOfWeekNumber);
            }
        }


        public DateTime StartedOn { get; }

        public DateTime FinishedOn { get; }


        internal Week GetNextWeek()
        {
            var nextWeekStartedOn = FinishedOn.AddDays(3);

            if (nextWeekStartedOn.Month != StartedOn.Month)
            {
                return null;
            }

            return new Week(nextWeekStartedOn);
        }

        

        internal string ToTxt()
        {
            var sb = new StringBuilder();
            
            sb.Append($"{StartedOn.Day}..{FinishedOn.Day}: ");

            var rateGropus = GroupRatesByCode();

            foreach (var g in rateGropus)
            {
                var min = g.Values.First();
                var max = g.Values.Last();
                var mediana = GetMediana(g.Values);
                sb.Append($"{g.Code} - max: {max.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}, min: {min.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}, mediana: {mediana.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}; ");
            }

            return sb.ToString();
        }

        public bool TryAddRate(Rate rate)
        {
            if (rate.Date >= StartedOn && rate.Date <= FinishedOn)
            {
                _rates.Add(rate);
                return true;
            }

            return false;
        }

        public IReadOnlyList<Rate> Rates => _rates;
    }

    public class Month
    {
        public Month(int year, int month, Rate[] rates)
        {
            var weeks = new List<Week>();
            var week = new Week(new DateTime(year, month, 1));

            do
            {
                weeks.Add(week);
                week = week.GetNextWeek();
            }
            while (week != null);

            Weeks = weeks.ToArray();

            foreach (var rate in rates)
            {
                foreach (var w in Weeks)
                {
                    if (w.TryAddRate(rate))
                    {
                        break;
                    }
                }
            }
        }

        public Week[] Weeks { get; private set; }


        public string ToTxt()
        {

            var sb = new StringBuilder();

            sb.AppendLine(string.Format("Year: {0:yyyy}, month: {0:MMMM}", Weeks[0].StartedOn));
            sb.AppendLine("Week periods:");

            foreach (var week in Weeks)
            {
                sb.AppendLine(week.ToTxt());
            }

            return sb.ToString();
        }
    }
}
