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
       
        public Task<IServiceResult<Error, ReportBuilder<string>>> GetTxtReportAsync(int year, int month)
        {
            return RunAsync(async () =>
            {
                var ratesResult = await _rateService.GetRatesAsync(year, month);

                if (ratesResult.IsFaulted)
                {
                    return ratesResult.TransformToFaultedResult<ReportBuilder<string>>();
                }

                ReportBuilder<string> monthWithWeeks = new TxtReportBuilder(year, month, ratesResult.Data);

                return ServiceResult<Error>.Success(monthWithWeeks);
            });
        }

        public Task<IServiceResult<Error, ReportBuilder<object>>> GetJsonReportAsync(int year, int month)
        {
            return RunAsync(async () =>
            {
                var ratesResult = await _rateService.GetRatesAsync(year, month);

                if (ratesResult.IsFaulted)
                {
                    return ratesResult.TransformToFaultedResult<ReportBuilder<object>>();
                }

                ReportBuilder<object> monthWithWeeks = new ObjectReportBuilder(year, month, ratesResult.Data);

                return ServiceResult<Error>.Success(monthWithWeeks);
            });
        }
    }
}
