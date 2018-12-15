using CurrencyRate.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyRate.Cnb
{

    public class CnbService : BaseService, IDisposable
    {
        private const string PATH_TEMPLATE_YEAR     = "/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year={0}";
        private const string PATH_TEMPLATE_DAILY    = "/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date={0}";
        private readonly HttpClient _httpClient;

        CnbRate[] ParseYearCsv(string csv)
        {
            return null;
        }

        public CnbService(CnbSettings settings, ILogger<CnbService> logger) : base(logger)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = settings.BaseAddress;
        }

      
        protected virtual Task<string> SendAsync(string urlPath)
        {
            return _httpClient.GetStringAsync(urlPath);
        }

        public Task<IServiceResult<CnbError, CnbRate[]>> GetYearRatesAsync(int year)
        {
            return RunAsync(async () =>
            {
                var csv = await SendAsync(string.Format(PATH_TEMPLATE_YEAR, year));
                var rates = ParseYearCsv(csv);
                return ServiceResult<CnbError>.Success(rates);
            });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
