using CurrencyRate.Cnb;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate.HistoricalDataService
{
    public class ConsoleAppHost : IDisposable
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly CnbService _cnbService;
        private readonly Settings _settings;
        private readonly ILogger<ConsoleAppHost> _logger;

        public ConsoleAppHost(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cnbService = serviceProvider.GetService<CnbService>();
            _settings = serviceProvider.GetService<Settings>();
            _logger = _serviceProvider.GetService<ILogger<ConsoleAppHost>>();
        }

        public void Dispose()
        {
            if (_cnbService != null)
            {
                _cnbService.Dispose();
            }
        }

        public async Task RunAsync()
        {
            var results = await Task.WhenAll(_settings.Years.Select(y => _cnbService.GetYearRatesAsync(y)));
            var faultedResult = results.FirstOrDefault(x => x.IsFaulted);
            if (faultedResult != null)
            {
                return;
            }
        }
    }
}
