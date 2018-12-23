using CurrencyRate.Cnb;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Ef;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace CurrencyRate.DailyRateService
{
    class Program
    {
        static void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static ServiceCollection ConfigureServices(IConfigurationRoot configuration)
        {
            var settings = new Settings(configuration);
            var services = new ServiceCollection();

            services.AddLogging(c => c.AddConsole());

            services.AddSingleton(settings);
            services.AddSingleton(settings.Cnb);
            services.AddTransient<CnbService>();

            services.AddTransient<IUnitOfWork>(sp => new UnitOfWork(new DataContext(settings.ConnectionString)));

            services.AddTransient<RateJob>();

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
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{environmentName}.json", optional: true)
                    .AddEnvironmentVariables()
                    .Build();

                serviceProvider = ConfigureServices(configuration)
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
                    host.StopAsync().Wait();
                }
                if (serviceProvider != null)
                {
                    serviceProvider.Dispose();
                }
            }
        }
    }
}
