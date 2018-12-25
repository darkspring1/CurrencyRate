using CurrencyRate.Api.ApplicationServices;
using CurrencyRate.Api.ApplicationServices.Reports;
using CurrencyRate.Api.Controllers;
using CurrencyRate.Api.Settings;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Ef;
using CurrencyRate.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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

        private Task<ActionResult> SendReportRequestAsync(int year, int month, string type)
        {
            var date = new DateTime(year, month, 1);

            var rate1 = Rate.Create("EUR", 25M, 1, date.AddDays(-1));
            var rate2 = Rate.Create("EUR", 25.620M, 1, date);
            var rate3 = Rate.Create("USD", 22.039M, 1, date);
            var rate4 = Rate.Create("GBP", 28.845M, 1, date);
            var rate5 = Rate.Create("IDR", 1.529M, 1000, date);
            var rate6 = Rate.Create("EUR", 25.1M, 1, date.AddMonths(1));

            SetData(rate1, rate2, rate3, rate4, rate5, rate6);

            var controller = _serviceProvider.GetRequiredService<RatesController>();

            return controller.Get(year, month, type);
        }

        public RatesControllerTests()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("test.appsettings.json")
            .AddEnvironmentVariables()
            .Build();

            var settings = new ApiSettings(configuration);

            var services = new ServiceCollection();

            services.AddLogging(c => c.AddConsole());

            var dataContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;


            services.AddSingleton(sp => settings);
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
            const string type = "txt";

            var actionResult = await SendReportRequestAsync(year, month, type);

            var report = MvcUtils.GetSuccessResultData<string>(actionResult);

            using (var reader = new StringReader(report))
            {
                var firstLine = reader.ReadLine();
                var secondLine = reader.ReadLine();
                var thirdLine = reader.ReadLine();

                Assert.Equal(string.Format("Year: {0:yyyy}, month: {0:MMMM}", new DateTime(year, month, 1)), firstLine);
                Assert.Equal("Week periods:", secondLine);
                Assert.Equal("1..2: EUR - max: 25.62, min: 25.62, media: 25.62; USD - max: 22.04, min: 22.04, media: 22.04; ", thirdLine);  
            }
        }


        [Fact]
        [Trait("category", TRAIT)]
        public async Task ShouldReturnJsonReport()
        {
            const int year = 2018;
            const int month = 2;
            const string type = "json";

            var actionResult = await SendReportRequestAsync(year, month, type);

            var report = MvcUtils.GetSuccessResultData<Report>(actionResult);

            Assert.Equal(year.ToString(), report.Year);
            Assert.Equal(new DateTime(year, month, 1).ToString("MMMM"), report.Month);
            Assert.Equal(5, report.WeekPeriods.Length);
        }
    }
}
