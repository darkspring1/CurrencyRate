using CurrencyRate.Domain.Entities;
using System;
using System.Globalization;
using System.Text;

namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public class TxtReportBuilder : ReportBuilder<string>
    {
        public TxtReportBuilder(int year, int month, Rate[] rates) : base(year, month, rates)
        {
        }

        public override string Build()
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("Year: {0:yyyy}, month: {0:MMMM}", Weeks[0].StartedOn));
            sb.AppendLine("Week periods:");

            foreach (var week in Weeks)
            {
                sb.Append($"{week.StartedOn.Day}..{week.FinishedOn.Day}: ");

                var rateInfos = week.GetRateInfos();

                foreach (var r in rateInfos)
                {
                    sb.Append($"{r.Code} - max: {r.Max.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}, min: {r.Min.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}, media: {r.Media.ToString(DECIMAL_FORMAT, CultureInfo.InvariantCulture)}; ");
                }
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }
}
