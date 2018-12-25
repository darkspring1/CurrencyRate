using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyRate.Api.ApplicationServices.Reports
{

    public class ReportBuilder
    {
        public ReportBuilder(int year, int month, Rate[] rates)
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

        public Report Build()
        {
            var weekPeriods = Weeks.Select(w => new WeekPeriod
            (
                startedOn: w.StartedOn.Day,
                finishedOn: w.FinishedOn.Day,
                rates: w.GetRateInfos()
            )).ToArray();

            return new Report(string.Format("{0:yyyy}", Weeks[0].StartedOn), string.Format("{0:MMMM}", Weeks[0].StartedOn), weekPeriods);
        }

        public Week[] Weeks { get; set; }
    }
}
