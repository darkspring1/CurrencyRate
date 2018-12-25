using CurrencyRate.Dal.Abstractions;
using CurrencyRate.Domain.Entities;
using System;
using System.Linq;

namespace CurrencyRate.Domain.Persistent.Specifications
{
    class Specifications
    {
        /// <summary>
        /// курсы за месяц
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public static ISpecification<Rate> MonthRate(int year, int month, string[] currencyCodes)
        {
            return new MonthRateSpecification(year, month, currencyCodes);
        }

        class MonthRateSpecification : ISpecification<Rate>
        {
            
            private readonly DateTime _fromDate;
            private readonly DateTime _tillDate;
            private readonly string[] _currencyCodes;

            public MonthRateSpecification(int year, int month, string[] currencyCodes)
            {
                _fromDate = new DateTime(year, month, 1).AddDays(-1);
                _tillDate = new DateTime(year, month + 1, 1);
                _currencyCodes = currencyCodes;
            }

            public IQueryable<Rate> Build(IQueryable<Rate> source)
            {
                return source.Where(r => r.Date > _fromDate && r.Date < _tillDate && _currencyCodes.Contains(r.Code));
            }
        }

    }
}
