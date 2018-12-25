using static CurrencyRate.Api.ApplicationServices.Reports.Week;

namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public class WeekPeriod
    {
        public WeekPeriod(int startedOn, int finishedOn, ReportRateInfo[] rates)
        {
            StartedOn = startedOn;
            FinishedOn = finishedOn;
            Rates = rates;
        }

        public int StartedOn { get; }
        public int FinishedOn { get; }
        public ReportRateInfo[] Rates { get; }
    }
}
