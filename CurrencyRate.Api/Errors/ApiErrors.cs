using CurrencyRate.Domain.Errors;

namespace CurrencyRate.Api.Errors
{

    public static class ApiErrors
    {
        /// <summary>
        /// Unknown report type
        /// </summary>
        /// <param name="badReportType"></param>
        /// <returns></returns>
        public static Error Error2001(string badReportType) => new Error(2001, $"Unknown report type '{badReportType}'.");
    }
}
