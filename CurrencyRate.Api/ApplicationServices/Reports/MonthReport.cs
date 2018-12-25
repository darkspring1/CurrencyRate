using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;

namespace CurrencyRate.Api.ApplicationServices.Reports
{

    public abstract class MonthReport
    {
        protected const string DECIMAL_FORMAT = "0.00";

        protected abstract string ToStr();

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

        public override string ToString()
        {
            return ToStr();
        }
    }
}
