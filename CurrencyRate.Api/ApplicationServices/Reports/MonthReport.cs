using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyRate.Api.ApplicationServices.Reports
{

    public abstract class MonthReport
    {
        public MonthReport(int year, int month, Rate[] rates)
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
