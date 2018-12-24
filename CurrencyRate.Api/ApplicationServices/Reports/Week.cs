﻿using CurrencyRate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyRate.Api.ApplicationServices.Reports
{
    public abstract class Week
    {
        protected const string DECIMAL_FORMAT = "0.00";
        private readonly List<Rate> _rates;
        private readonly Func<DateTime, Week> _weekCtor;

        protected decimal GetMediana(decimal[] values)
        {
            var r = values.Length % 2;
            if (r == 0)
            {
                var i2 = values.Length / 2;
                var i1 = i2 - 1;

                return (values[i1] + values[i2]) / 2;
            }

            return values[values.Length / 2];
        }

        protected (string Code, decimal[] Values)[] GroupRatesByCode()
        {

            return Rates
                .GroupBy(x => x.Code)
                .Select(g =>
                (
                    Code: g.Key,
                    Values: g.Select(rate => rate.Value / rate.Amount).OrderBy(val => val).ToArray()
                ))
                .ToArray();
        }

        protected abstract Week Create(DateTime startedOn);

        protected abstract string ToStr();

        protected Week(DateTime startedOn)
        {
            _rates = new List<Rate>();
            StartedOn = startedOn;
            
            //если месяц начинается в СБ или ВС
            if (startedOn.DayOfWeek == DayOfWeek.Saturday)
            {
                StartedOn = StartedOn.AddDays(2);
            }
            else if(startedOn.DayOfWeek == DayOfWeek.Sunday)
            {
                StartedOn = StartedOn.AddDays(1);
            }

            
            var daysInMonth = DateTime.DaysInMonth(StartedOn.Year, StartedOn.Month);


            int diff = 5 - (int)StartedOn.DayOfWeek;

            if (diff >= 0)
            {
                int lastDayOfWeekNumber = StartedOn.Day + diff;
                lastDayOfWeekNumber = lastDayOfWeekNumber > daysInMonth ? daysInMonth : lastDayOfWeekNumber;
                FinishedOn = new DateTime(StartedOn.Year, StartedOn.Month, lastDayOfWeekNumber);
            }
        }

        public DateTime StartedOn { get; }

        public DateTime FinishedOn { get; }

        internal Week GetNextWeek()
        {
            var nextWeekStartedOn = FinishedOn.AddDays(3);

            if (nextWeekStartedOn.Month != StartedOn.Month)
            {
                return null;
            }

            return Create(nextWeekStartedOn);
        }

        public override string ToString()
        {
            return ToStr();
        }

        public bool TryAddRate(Rate rate)
        {
            if (rate.Date >= StartedOn && rate.Date <= FinishedOn)
            {
                _rates.Add(rate);
                return true;
            }

            return false;
        }

        public IReadOnlyList<Rate> Rates => _rates;
    }
}
