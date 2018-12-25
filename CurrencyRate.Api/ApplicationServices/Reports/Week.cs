using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public class Week
    {
        internal class ReportRateInfo
        {
            public ReportRateInfo(string code, decimal min, decimal max, decimal media)
            {
                Code = code;
                Min = min;
                Max = max;
                Media = media;
            }

            public string Code { get; }
            public decimal Min { get; }
            public decimal Max { get; }
            public decimal Media { get; }
        }

        private readonly List<Rate> _rates;

        protected decimal GetMedia(decimal[] values)
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

        protected (string Code, decimal[] Values)[] GroupRatesByCode()
        {
            return _rates
                .GroupBy(x => x.Code)
                .Select(g =>
                (
                    Code: g.Key,
                    Values: g.Select(rate => rate.Value / rate.Amount).OrderBy(val => val).ToArray()
                ))
                .ToArray();
        }

        internal List<ReportRateInfo> GetRateInfos()
        {
            var rateGropus = GroupRatesByCode();

            var result = new List<ReportRateInfo>();
            foreach (var g in rateGropus)
            {
                var min = g.Values.First();
                var max = g.Values.Last();
                var media = GetMedia(g.Values);

                result.Add(new ReportRateInfo(g.Code, min, max, media));
            }

            return result;
        }

        internal Week(DateTime startedOn)
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
        
        internal bool TryAddRate(Rate rate)
        {
            if (rate.Date >= StartedOn && rate.Date <= FinishedOn)
            {
                _rates.Add(rate);
                return true;
            }

            return false;
        }

        //public IReadOnlyList<Rate> Rates => _rates;
    }
}
