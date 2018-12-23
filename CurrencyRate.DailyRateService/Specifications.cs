using CurrencyRate.Dal.Abstractions;
using CurrencyRate.Domain.Entities;
using System;
using System.Linq;

namespace CurrencyRate.DailyRateService
{
    class Specifications
    {
        public static ISpecification<Rate> DailyRate(DateTime date)
        {
            return new DailyRateSpecification(date);
        }

        class DailyRateSpecification : ISpecification<Rate>
        {
            private readonly DateTime _date;

            public DailyRateSpecification(DateTime date)
            {
                _date = date;
            }

            public IQueryable<Rate> Build(IQueryable<Rate> source)
            {
                return source.Where(r => r.Date == _date.Date);
            }
        }

    }
}
