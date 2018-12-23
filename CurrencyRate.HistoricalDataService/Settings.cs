using CurrencyRate.Cnb;
using Microsoft.Extensions.Configuration;
using Rkl.Settings;

namespace CurrencyRate.HistoricalDataService
{
    class Settings : ConfigSection
    {
        public Settings(IConfiguration configuration) : base(configuration)
        {
        }


        public string ConnectionString => GetString("ConnectionString");

        public CnbSettings Cnb => GetSection<CnbSettings>("Cnb");

        /// <summary>
        /// годы за который стоит выгрузить данные
        /// </summary>
        public int[] Years => GetArray("Years", int.Parse);
    }
}
