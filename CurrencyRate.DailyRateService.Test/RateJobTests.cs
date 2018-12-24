using CurrencyRate.Cnb;
using CurrencyRate.DailyRateService.Test.TestServices;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Ef;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyRate.DailyRateService.Test
{
    public class RateJobTests : IDisposable
    {
        const string TRAIT = "DailyRateService.Unit";

        private readonly ServiceProvider _serviceProvider;

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

        public RateJobTests()
        {
            var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();

            var settings = new Settings(configuration);
            var services = new ServiceCollection();

            services.AddLogging(c => c.AddConsole());

            //services.AddSingleton(settings);
            services.AddSingleton(settings.Cnb);
            services.AddTransient<CnbService, TestCnbService>();


            var dataContextOptions = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            services.AddSingleton(sp => new DataContext(dataContextOptions));

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<RateJob>();
            _serviceProvider = services.BuildServiceProvider();
        }


        
        /// <summary>
        /// Должен обновлять курс, если он уже есть в БД
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("category", TRAIT)]
        public async Task ShouldUpdateRate()
        {
            var date = DateTime.UtcNow.Date;

            var rateForUpdate = Rate.Create(code: "EUR", date: date, value: 25, amount: 1);
            decimal newValue = 25.620M;

            SetData(rateForUpdate);

            TestCnbService.SetRates(new CnbRate(date: date, code: "EUR", value: newValue, amount: 1));

            var job = new RateJob(_serviceProvider);
            await job.Execute(null);

            var rates = GetData<Rate>();

            Assert.Single(rates);
            Assert.Equal(newValue, rates.Single().Value);
        }

        /// <summary>
        /// Должен создать курс, если его нет
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("category", TRAIT)]
        public async Task ShouldCreateRate()
        {
            var date = DateTime.UtcNow.Date;

            decimal newValue = 25.620M;

            TestCnbService.SetRates(new CnbRate(date: date, code: "EUR", value: newValue, amount: 1));

            var job = new RateJob(_serviceProvider);
            await job.Execute(null);

            var rates = GetData<Rate>();

            Assert.Single(rates);
            Assert.Equal(newValue, rates.Single().Value);
        }

        public void Dispose()
        {
            if (_serviceProvider != null)
            {
                _serviceProvider.Dispose();
            }
        }
    }
}
