using System;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public class WeekTxt : Week
    {
        public WeekTxt(DateTime startedOn) : base(startedOn)
        {
        }

        protected override Week Create(DateTime startedOn)
        {
            return new WeekTxt(startedOn);
        }

        protected override string ToStr()
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
    }
}
