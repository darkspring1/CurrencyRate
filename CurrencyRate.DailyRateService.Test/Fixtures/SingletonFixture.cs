﻿using CurrencyRate.Cnb;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using Xunit;

namespace CurrencyRate.DailyRateService.Test.Fixtures
{
    /// <summary>
    /// Объекты в этой Fixture, буду созданы один раз перед всеми тестами.
    /// </summary>
    public class SingletonFixture : IDisposable
    {
        public SingletonFixture()
        {
            var config = new ConfigurationBuilder()
              .AddJsonFile("appsettings.tests.json")
              .Build();

            Settings = new CnbSettings(config, "cnb");
            LoggerFactory = new LoggerFactory();
        }

        public LoggerFactory LoggerFactory { get; }

        public CnbSettings Settings { get; }

        public void Dispose()
        {
            LoggerFactory.Dispose();
        }
    }


    [CollectionDefinition(SingletonCollection.Name)]
    public class SingletonCollection : ICollectionFixture<SingletonFixture>
    {
        public const string Name = "SingletonСollection";
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
