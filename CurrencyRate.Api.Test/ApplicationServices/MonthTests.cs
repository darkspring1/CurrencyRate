using CurrencyRate.Api.ApplicationServices;
using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace CurrencyRate.Api.Test.ApplicationServices
{
    public class MonthTests
    {

        List<DateTime[]> GenerateWeeks(int year, int month)
        {
            var m = new Month(year, month, new Rate[0]);
            var result = new List<DateTime[]>();
            foreach (var w in m.Weeks)
            {
                var dates = new DateTime[(w.FinishedOn - w.StartedOn).Days + 1];
                for (int i = 0; i < dates.Length; i++)
                {
                    dates[i] = w.StartedOn.AddDays(i);
                }
                result.Add(dates);
            }

            return result;
        }


        Rate[] GetFebruaryRates()
        {
            const int year = 2018;
            const int month = 2;

            var usdRates = new[] { 22.039M, 22.1M, 22.09M, 22.7M, 22.4M, };
            var eurRates = new[] { 25.62M, 25.53M, 25.61M, 25.2M, 25.6M, };
            var phpRates = new[] { 41.34M, 41.2M, 41.48M, 41.328M, 41.3M, };
            
            var weeks = GenerateWeeks(year, month);

            var rates = new List<Rate>();

            foreach (var w in weeks)
            {
                foreach (var date in w)
                {
                    var index = date.Day % (usdRates.Length - 1);

                    var usd = Rate.Create(code: "USD", value: usdRates[index], amount: 1, date: date);
                    var eur = Rate.Create(code: "EUR", value: eurRates[index], amount: 1, date: date);
                    var php = Rate.Create(code: "PHP", value: phpRates[index], amount: 100, date: date);

                    rates.AddRange(new[] { usd, eur, php });
                }
            }

            return rates.ToArray();
        }

        class ExpectedWeek
        {
            public ExpectedWeek(int startedOn, int finishedOn)
            {
                StartedOn = startedOn;
                FinishedOn = finishedOn;
            }

            public int StartedOn { get; }
            public int FinishedOn { get; }
        }

        class ExpectedMonth
        {
            public ExpectedMonth(int number, ExpectedWeek[] weeks)
            {
                Number = number;
                Weeks = weeks;
            }
            public int Number { get; }
            public ExpectedWeek[] Weeks { get; }
        }

        /// <summary>
        /// Проверяем, как работает конструктор
        /// </summary>
        [Fact]
        public void ShouldCreateMonthWithWeeks()
        {
            var expectedMonths = new[]
            {
                new ExpectedMonth(
                    number: 1,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 5),
                        new ExpectedWeek(8, 12),
                        new ExpectedWeek(15, 19),
                        new ExpectedWeek(22, 26),
                        new ExpectedWeek(29, 31)
                    }),

              

                new ExpectedMonth(
                    number: 2,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 2),
                        new ExpectedWeek(5, 9),
                        new ExpectedWeek(12, 16),
                        new ExpectedWeek(19, 23),
                        new ExpectedWeek(26, 28)
                    }),
                new ExpectedMonth(
                    number: 3,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 2),
                        new ExpectedWeek(5, 9),
                        new ExpectedWeek(12, 16),
                        new ExpectedWeek(19, 23),
                        new ExpectedWeek(26, 30)
                    }),
                new ExpectedMonth(
                    number: 4,
                    weeks: new []
                    {
                        new ExpectedWeek(2, 6),
                        new ExpectedWeek(9, 13),
                        new ExpectedWeek(16, 20),
                        new ExpectedWeek(23, 27)
                    }),
                new ExpectedMonth(
                    number: 5,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 4),
                        new ExpectedWeek(7, 11),
                        new ExpectedWeek(14, 18),
                        new ExpectedWeek(21, 25),
                        new ExpectedWeek(28, 31)
                    }),
                new ExpectedMonth(
                    number: 6,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 1),
                        new ExpectedWeek(4, 8),
                        new ExpectedWeek(11, 15),
                        new ExpectedWeek(18, 22),
                        new ExpectedWeek(25, 29)
                    }),
                new ExpectedMonth(
                    number: 7,
                    weeks: new []
                    {
                        new ExpectedWeek(2, 6),
                        new ExpectedWeek(9, 13),
                        new ExpectedWeek(16, 20),
                        new ExpectedWeek(23, 27)
                    }),
                new ExpectedMonth(
                    number: 8,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 3),
                        new ExpectedWeek(6, 10),
                        new ExpectedWeek(13, 17),
                        new ExpectedWeek(20, 24),
                        new ExpectedWeek(27, 31)
                    }),
                new ExpectedMonth(
                    number: 9,
                    weeks: new []
                    {
                        new ExpectedWeek(3, 7),
                        new ExpectedWeek(10, 14),
                        new ExpectedWeek(17, 21),
                        new ExpectedWeek(24, 28)
                    }),
                new ExpectedMonth(
                    number: 10,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 5),
                        new ExpectedWeek(8, 12),
                        new ExpectedWeek(15, 19),
                        new ExpectedWeek(22, 26),
                        new ExpectedWeek(29, 31)
                    }),

                 new ExpectedMonth(
                    number: 11,
                    weeks: new []
                    {
                        new ExpectedWeek(1, 2),
                        new ExpectedWeek(5, 9),
                        new ExpectedWeek(12, 16),
                        new ExpectedWeek(19, 23),
                        new ExpectedWeek(26, 30)
                    }),

                  new ExpectedMonth(
                    number: 12,
                    weeks: new []
                    {
                        new ExpectedWeek(3, 7),
                        new ExpectedWeek(10, 14),
                        new ExpectedWeek(17, 21),
                        new ExpectedWeek(24, 28)
                    }),
            };

            foreach (var expectedMonth in expectedMonths)
            {
                try
                {
                    var actualMonth = new Month(2018, expectedMonth.Number, new Domain.Entities.Rate[0]);
                    for (int i = 0; i < expectedMonth.Weeks.Length; i++)
                    {
                        var expectedWeek = expectedMonth.Weeks[i];
                        var actualWeek = actualMonth.Weeks[i];

                        Assert.Equal(expectedWeek.StartedOn, actualWeek.StartedOn.Day);
                        Assert.Equal(expectedWeek.FinishedOn, actualWeek.FinishedOn.Day);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception($"Promblem with month number: {expectedMonth.Number}", e);
                }
            }
        }

        [Fact]
        public void ShouldReturnTxtReport()
        {
            const int year = 2018;
            const int month = 2;

            var rates = GetFebruaryRates();
            var actualMonth = new Month(year, month, rates);
            var report = actualMonth.ToTxt();

            using (var reader = new StringReader(report))
            {
                var firstLine = reader.ReadLine();
                var secondLine = reader.ReadLine();
                var thirdLine = reader.ReadLine();

                Assert.Equal("1..2: USD - max: 22.10, min: 22.09, mediana: 22.10; EUR - max: 25.61, min: 25.53, mediana: 25.57; PHP - max: 0.41, min: 0.41, mediana: 0.41; ", thirdLine);
            }
        }
    }
}
