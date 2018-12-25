using Microsoft.Extensions.Configuration;
using Rkl.Settings;

namespace CurrencyRate.Api.Settings
{
    public class ApiSettings : ConfigSection
    {

        public ApiSettings(IConfiguration configuration) : base(configuration)
        {
            
        }

        public string ConnectionString => GetString("ConnectionString");

        public string[] CurrencyCodes => GetArray(nameof(CurrencyCodes), s => s);

    }
}
