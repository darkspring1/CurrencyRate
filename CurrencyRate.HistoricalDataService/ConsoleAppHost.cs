using CurrencyRate.Cnb;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Persistent;
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
        private readonly IUnitOfWork _unitOfWork;

        public ConsoleAppHost(ServiceProvider serviceProvider)
        {
            _serviceProvider    = serviceProvider;
            _cnbService         = serviceProvider.GetRequiredService<CnbService>();
            _settings           = serviceProvider.GetRequiredService<Settings>();
            _logger             = _serviceProvider.GetRequiredService<ILogger<ConsoleAppHost>>();
            _unitOfWork         = _serviceProvider.GetRequiredService<IUnitOfWork>();
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
            _logger.LogInformation("Loading historical data...");

            var results = await Task.WhenAll(_settings.Years.Select(y => _cnbService.GetYearRatesAsync(y)));
            var faultedResult = results.FirstOrDefault(x => x.IsFaulted);
            if (faultedResult != null)
            {
                return;
            }

            _logger.LogInformation("Historical data was loaded.");

            _logger.LogInformation("Saving data to Db.");

            var rates = results
                .SelectMany(r => r.Data)
                .Select(r => Rate.Create(r.Code, r.Value, r.Date))
                .ToArray();

            _unitOfWork.RateRepository.Add(rates);

            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Data was saved to db.");

        }
    }
}
