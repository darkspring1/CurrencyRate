using CurrencyRate.Cnb;
using Microsoft.Extensions.Configuration;
using Rkl.Settings;

namespace CurrencyRate.DailyRateService
{
    class Settings : ConfigSection
    {
        public Settings(IConfiguration configuration) : base(configuration)
        {
        }

        public string ConnectionString => GetString("ConnectionString");

        public CnbSettings Cnb => GetSection<CnbSettings>("Cnb");
    }
}
