using CurrencyRate.Abstractions;
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
        /*
        object ByWeeks(Rate[] rates)
        {
            GregorianCalendar
            CalendarWeekRule.FirstFourDayWeek
            rates[0].Date.D
        }
        */

        public Task<IServiceResult<Error, Month>> GetRatesAsync(int year, int month, string format)
        {
            return RunAsync(async () =>
            {
                var ratesResult = await _rateService.GetRatesAsync(year, month);

                var monthWithWeeks = new Month(year, month, ratesResult.Data);

                return ServiceResult<Error>.Success(monthWithWeeks);
            });
        }
    }
}
