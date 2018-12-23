using CurrencyRate.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace CurrencyRate.Cnb
{

    public class CnbService : BaseService, IDisposable
    {
        private const string PATH_TEMPLATE_YEAR     = "/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year={0}";
        private const string PATH_TEMPLATE_DAILY    = "/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date={0}";
        private readonly NumberFormatInfo _numberFormatInfo;
        private readonly HttpClient _httpClient;

        CnbRate[] ParseDay(DateTime date, string response)
        {
            var strings = response.Split("\n").Skip(2);
            var result = new List<CnbRate>();
            foreach (var str in strings)
            {
                if (str != "")
                {
                    var strData = str.Split('|');
                    var rate = new CnbRate(
                        date: date,
                        value: decimal.Parse(strData[4], _numberFormatInfo),
                        code: strData[3],
                        amount: int.Parse(strData[2]));

                    result.Add(rate);
                }
            }

            return result.ToArray();
        }

        CnbRate[] ParseYear(string response)
        {
            var strings = response.Split("\n");

            var codesWithAmount = strings[0]
                .Split('|')
                .Skip(1)
                .Select(x =>
                {
                    var codeWithAmount = x.Split(' ');
                    return (Amount: int.Parse(codeWithAmount[0]), Code: codeWithAmount[1]);
                })
                .ToArray();

            
            var result = new List<CnbRate>();

            for (int i = 1; i < strings.Length; i++)
            {
                if (strings[i] != "")
                {
                    var strData = strings[i].Split('|');
                    
                    var date = DateTime.Parse(strData[0]);
                    for (int j = 1; j < strData.Length; j++)
                    {
                        var rate = new CnbRate(
                            value: decimal.Parse(strData[j], _numberFormatInfo),
                            code: codesWithAmount[j - 1].Code,
                            amount: codesWithAmount[j - 1].Amount,
                            date: date);

                        result.Add(rate);
                    }
                }
            }

            return result.ToArray();
        }

        public CnbService(CnbSettings settings, ILogger<CnbService> logger) : base(logger)
        {
            _numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
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
                var str = await SendAsync(string.Format(PATH_TEMPLATE_YEAR, year));
                var rates = ParseYear(str);
                return ServiceResult<CnbError>.Success(rates);
            });
        }

        /// <summary>
        /// Дневной курс
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public virtual Task<IServiceResult<CnbError, CnbRate[]>> GetDailyRatesAsync(DateTime date)
        {
            return RunAsync(async () =>
            {
                var dateParam = date.Date.ToString("dd.MM.yyyy");
                var str = await SendAsync(string.Format(PATH_TEMPLATE_DAILY, dateParam));
                var rates = ParseDay(date.Date, str);
                return ServiceResult<CnbError>.Success(rates);
            });
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
