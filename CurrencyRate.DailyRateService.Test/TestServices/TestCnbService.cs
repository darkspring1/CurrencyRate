using System;
using System.Threading.Tasks;
using CurrencyRate.Abstractions;
using CurrencyRate.Cnb;
using Microsoft.Extensions.Logging;

namespace CurrencyRate.DailyRateService.Test.TestServices
{
    class TestCnbService : CnbService
    {
        private static CnbRate[] _rates;
        public TestCnbService(CnbSettings settings, ILogger<CnbService> logger) : base(settings, logger)
        {
        }

        public static void SetRates(params CnbRate[] rates)
        {
            _rates = rates;
        }

        public override Task<IServiceResult<CnbError, CnbRate[]>> GetDailyRatesAsync(DateTime date)
        {
            return Task.FromResult(ServiceResult<CnbError>.Success(_rates));
        }
    }
}
