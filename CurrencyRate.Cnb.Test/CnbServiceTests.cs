using CurrencyRate.Cnb.Test.Fixtures;
using CurrencyRate.Cnb.Test.TestServices;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace CurrencyRate.Cnb.Test
{
    [Collection(Constants.Fixtures.SingletonСollection)]
    public class CnbServiceTests
    {
        const string TRAIT = "Cnb.Unit";
        private readonly CnbSettings _settings;
        private readonly LoggerFactory _loggerFactory;


        void AssertCnbRate(DateTime expectedDate, decimal expectedValue, int expectedAmount, string expectedCode, CnbRate actualRate)
        {
            Assert.Equal(expectedDate, actualRate.Date);
            Assert.Equal(expectedValue, actualRate.Value);
            Assert.Equal(expectedAmount, actualRate.Amount);
            Assert.Equal(expectedCode, actualRate.Code);
        }

        ILogger<T> CreateLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

        TestCnbService CreateCnbService(string fileName)
        {
            var response = File.ReadAllText($"data/{fileName}");
            return new TestCnbService(response, _settings, CreateLogger<CnbService>());
        }

        public CnbServiceTests(SingletonFixture singletonFixture)
        {
            _settings = singletonFixture.Settings;
            _loggerFactory = singletonFixture.LoggerFactory;
        }


        /// <summary>
        /// Успешный сценарий получения курсов за год
        /// </summary>
        /// <returns></returns>
        [Fact]
        [Trait("category", TRAIT)]
        public async Task ShouldReturnYearRates()
        {
            using (var cnb = CreateCnbService("year.txt"))
            {
                var result = await cnb.GetYearRatesAsync(2018);
                var rates = result.Data;

                Assert.Equal(132, rates.Length);

                AssertCnbRate(new DateTime(2018, 1, 2), 16.540M, 1, "AUD", rates[0]);
                AssertCnbRate(new DateTime(2018, 1, 2), 8.262M, 100, "HUF", rates[11]);
                AssertCnbRate(new DateTime(2018, 12, 14), 1.590M, 1, "ZAR", rates[131]);
            }
        }
    }
}
