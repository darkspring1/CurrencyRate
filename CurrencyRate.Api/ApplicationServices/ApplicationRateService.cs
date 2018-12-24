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
       
        public Task<IServiceResult<Error, MonthReport>> GetRatesAsync(int year, int month, string format)
        {
            return RunAsync(async () =>
            {
                var ratesResult = await _rateService.GetRatesAsync(year, month);

                MonthReport monthWithWeeks = new MonthReportTxt(year, month, ratesResult.Data);

                return ServiceResult<Error>.Success(monthWithWeeks);
            });
        }
    }
}
