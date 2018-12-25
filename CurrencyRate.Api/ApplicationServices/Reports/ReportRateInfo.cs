namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public class ReportRateInfo
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
}
