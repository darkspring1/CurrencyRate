using CurrencyRate.Domain.Entities;
using System;
using System.Text;

namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public class MonthReportTxt : MonthReport
    {
        public MonthReportTxt(int year, int month, Rate[] rates) : base(year, month, rates)
        {
        }

        protected override Week CreateWeek(DateTime startedOn)
        {
            return new WeekTxt(startedOn);
        }

        protected override string ToStr()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("Year: {0:yyyy}, month: {0:MMMM}", Weeks[0].StartedOn));
            sb.AppendLine("Week periods:");

            foreach (var week in Weeks)
            {
                sb.AppendLine(week.ToString());
            }

            return sb.ToString();
        }
    }
}
