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
        public static ISpecification<Rate> MonthRate(int year, int month)
        {
            return new MonthRateSpecification(year, month);
        }

        class MonthRateSpecification : ISpecification<Rate>
        {
            
            private readonly DateTime _fromDate;
            private readonly DateTime _tillDate;

            public MonthRateSpecification(int year, int month)
            {
                _fromDate = new DateTime(year, month, 1).AddDays(-1);
                _tillDate = new DateTime(year, month + 1, 1);
            }

            public IQueryable<Rate> Build(IQueryable<Rate> source)
            {
                return source.Where(r => r.Date > _fromDate && r.Date < _tillDate);
            }
        }

    }
}
