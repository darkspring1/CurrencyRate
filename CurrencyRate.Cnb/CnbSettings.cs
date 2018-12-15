using Microsoft.Extensions.Configuration;
using Rkl.Settings;
using System;

namespace CurrencyRate.Cnb
{
    public class CnbSettings : ConfigSection
    {

        string Schema => GetString("Schema");
        string Host => GetString("Host");
        int Port => GetInt("Port");

        public CnbSettings(IConfiguration configuration, string keyPrefix) : base(configuration, keyPrefix)
        {
        }
        
        public Uri BaseAddress => new Uri($"{Schema}://{Host}:{Port}");
    }
}
