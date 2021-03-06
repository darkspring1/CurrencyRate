﻿using CurrencyRate.Cnb;
using Microsoft.Extensions.Configuration;
using Rkl.Settings;

namespace CurrencyRate.DailyRateService
{
    public class Settings : ConfigSection
    {
        public Settings(IConfiguration configuration) : base(configuration)
        {
        }

        public string ConnectionString => GetString("ConnectionString");

        public CnbSettings Cnb => GetSection<CnbSettings>("Cnb");

        /// <summary>
        /// Настройка для запуска по Cron
        /// </summary>
        public string CronExpression => GetString(nameof(CronExpression));
    }
}
