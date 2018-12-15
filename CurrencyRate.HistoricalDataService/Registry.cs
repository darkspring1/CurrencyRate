using CurrencyRate.Domain.Persistent.Ef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyRate.HistoricalDataService
{
    class Registry
    {
        private readonly IConfiguration _configuration;

        public Registry(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ServiceCollection ConfigureServices()
        {
            var settings = new Settings(_configuration);
            var services = new ServiceCollection();
            //services.AddSingleton(settings);
            services.AddSingleton(settings.Cnb);
            services.AddTransient(sp => new DataContext(settings.ConnectionString));
            
            return services;
        }
    }
}
