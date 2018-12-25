using System;
using System.Globalization;
using System.Text;

namespace CurrencyRate.Api.ApplicationServices.Reports
{

    public class Report
    {
        const string DECIMAL_FORMAT = "0.00";

        public Report(string year, string month, WeekPeriod[] weekPeriods)
        {
            Year = year;
            Month = month;
            WeekPeriods = weekPeriods;
        }

        public string Year { get; }
        public string Month { get; }
        public WeekPeriod[] WeekPeriods { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Year: {Year}, month: {Month}");
            sb.AppendLine("Week periods:");

            foreach (var week in WeekPeriods)
            {
                sb.Append($"{week.StartedOn}..{week.FinishedOn}: ");

                foreach (var r in week.Rates)
                {
                    sb.Append($"{r.Code} - max: {r.Max.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}, min: {r.Min.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}, media: {r.Media.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}; ");
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
