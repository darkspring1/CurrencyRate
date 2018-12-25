using CurrencyRate.Abstractions;
using CurrencyRate.Api.ApplicationServices.Reports;
using CurrencyRate.Domain.Errors;
using CurrencyRate.Domain.Services;
using System.Threading.Tasks;

namespace CurrencyRate.Api.ApplicationServices
{
    public class ApplicationRateService : BaseService
    {
        private readonly RateService _rateService;

        public ApplicationRateService(RateService rateService)
        {
            _rateService = rateService;
        }
       
        public Task<IServiceResult<Error, string>> GetTxtReportAsync(int year, int month)
        {
            return RunAsync(async () =>
            {
                var ratesResult = await _rateService.GetRatesAsync(year, month);

                if (ratesResult.IsFaulted)
                {
                    return ratesResult.TransformToFaultedResult<string>();
                }

                var builder = new ReportBuilder(year, month, ratesResult.Data);
                var report = builder.Build();
                return ServiceResult<Error>.Success(report.ToString());
            });
        }

        public Task<IServiceResult<Error, Report>> GetReportAsync(int year, int month)
        {
            return RunAsync(async () =>
            {
                var ratesResult = await _rateService.GetRatesAsync(year, month);

                if (ratesResult.IsFaulted)
                {
                    return ratesResult.TransformToFaultedResult<Report>();
                }

                var builder = new ReportBuilder(year, month, ratesResult.Data);
                var report = builder.Build();

                return ServiceResult<Error>.Success(report);
            });
        }
    }
}
