using CurrencyRate.Api.ApplicationServices;
using CurrencyRate.Api.Controllers;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Ef;
using CurrencyRate.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyRate.Api.Test
{
    public class RatesControllerTests
    {
        private readonly ServiceProvider _serviceProvider;

        const string TRAIT = "WebApi.RatesController.Unit";

        private void SetData(params object[] entities)
        {
            var ctx = _serviceProvider.GetRequiredService<DataContext>();
            ctx.AddRange(entities);
            ctx.SaveChanges();
        }

        private TEntity[] GetData<TEntity>() where TEntity : class
        {
            var ctx = _serviceProvider.GetRequiredService<DataContext>();
            return ctx.Set<TEntity>().ToArray();
        }

        public RatesControllerTests()
        {
        //    var configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.test.json")
        //    .AddEnvironmentVariables()
        //    .Build();

            //var settings = new ApiSettings(configuration);

            var services = new ServiceCollection();

            services.AddLogging(c => c.AddConsole());

            var dataContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            services.AddSingleton(sp => new DataContext(dataContextOptions));

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<ApplicationRateService, ApplicationRateService>();
            services.AddTransient<RateService, RateService>();
            services.AddTransient<RatesController, RatesController>();
            
            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        [Trait("category", TRAIT)]
        public async Task ShouldReturnTxtReport()
        {
            const int year = 2018;
            const int month = 2;

            var date = new DateTime(year, month, 1);

            var rate1 = Rate.Create("EUR", 25M, 1, date.AddDays(-1));
            var rate2 = Rate.Create("EUR", 25.620M, 1, date);
            var rate3 = Rate.Create("USD", 22.039M, 1, date);
            var rate4 = Rate.Create("GBP", 28.845M, 1, date);
            var rate5 = Rate.Create("IDR", 1.529M, 1000, date);
            var rate6 = Rate.Create("EUR", 25.1M, 1, date.AddMonths(1));

            SetData(rate1, rate2, rate3, rate4, rate5, rate6);

            var controller = _serviceProvider.GetRequiredService<RatesController>();

            await controller.Get(2018, 2, "txt");
        }


        [Fact]
        [Trait("category", TRAIT)]
        public async Task ShouldReturnJsonReport()
        {
            const int year = 2018;
            const int month = 2;

            var date = new DateTime(year, month, 1);

            var rate1 = Rate.Create("EUR", 25M, 1, date.AddDays(-1));
            var rate2 = Rate.Create("EUR", 25.620M, 1, date);
            var rate3 = Rate.Create("USD", 22.039M, 1, date);
            var rate4 = Rate.Create("GBP", 28.845M, 1, date);
            var rate5 = Rate.Create("IDR", 1.529M, 1000, date);
            var rate6 = Rate.Create("EUR", 25.1M, 1, date.AddMonths(1));

            SetData(rate1, rate2, rate3, rate4, rate5, rate6);

            var controller = _serviceProvider.GetRequiredService<RatesController>();

            var result = await controller.Get(2018, 2, "json");
        }
    }
}
