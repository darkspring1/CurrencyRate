using CurrencyRate.Domain.Entities;
using System.Linq;

namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public class ObjectReportBuilder : ReportBuilder<object>
    {
        public ObjectReportBuilder(int year, int month, Rate[] rates) : base(year, month, rates)
        {
        }

        public override object Build()
        {
            var weekPeriods = Weeks.Select(w => new
            {
                StartedOn = w.StartedOn,
                FinishedOn = w.FinishedOn,
                Rates = w.GetRateInfos()
            });

            return new
            {
                Year = string.Format("{0:yyyy}", Weeks[0].StartedOn),
                Month = string.Format("{0:MMMM}", Weeks[0].StartedOn),
                WeekPeriods = weekPeriods
            };
        }
    }
}
