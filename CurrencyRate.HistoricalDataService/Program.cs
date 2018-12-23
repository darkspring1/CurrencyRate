using CurrencyRate.Cnb;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Ef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CurrencyRate.HistoricalDataService
{
    /// <summary>
    /// Загрузка исторических данных
    /// </summary>
    class Program
    {
        private static IConfigurationRoot _configuration;

        static void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static ServiceCollection ConfigureServices()
        {
            var settings = new Settings(_configuration);
            var services = new ServiceCollection();

            services.AddLogging(c => c.AddConsole());

            services.AddSingleton(settings);
            services.AddSingleton(settings.Cnb);
            services.AddTransient<CnbService>();

            services.AddTransient<IUnitOfWork>(sp => new UnitOfWork(new DataContext(settings.ConnectionString)));

            return services;
        }

        static void Main(string[] args)
        {
            ConsoleAppHost host = null;
            ServiceProvider serviceProvider = null;
            try
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Console.WriteLine($"Hosting environment: {environmentName}");
                _configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{environmentName}.json")
                    .AddEnvironmentVariables()
                    .Build();

                serviceProvider = ConfigureServices()
                    .BuildServiceProvider();

                host = new ConsoleAppHost(serviceProvider);
                host.RunAsync().Wait();
                PressAnyKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                PressAnyKey();
            }
            finally
            {
                if (host != null)
                {
                    host.Dispose();
                }
                if (serviceProvider != null)
                {
                    serviceProvider.Dispose();
                }
            }

        }
    }
}
