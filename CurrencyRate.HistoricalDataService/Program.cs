using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CurrencyRate.HistoricalDataService
{

    class Program
    {

        static void PressAnyKey()
        {
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            ConsoleAppHost host = null;
            ServiceProvider serviceProvider = null;
            try
            {
                var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                Console.WriteLine($"Hosting environment: {environmentName}");
                var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddJsonFile($"appsettings.{environmentName}.json")
                    .AddEnvironmentVariables()
                    .Build();

                var registry = new Registry(config);
                serviceProvider = registry
                    .ConfigureServices()
                    .BuildServiceProvider();
                host = new ConsoleAppHost(serviceProvider);
                host.RunAsync();
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
