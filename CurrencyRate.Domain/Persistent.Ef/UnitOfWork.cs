using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using CurrencyRate.Dal.Abstractions;
using CurrencyRate.Dal.Ef;
using CurrencyRate.Domain.Entities;

namespace CurrencyRate.Domain.Persistent.Ef
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        private readonly Lazy<IRepository<Rate>> _rateRepository;

        public IRepository<Rate> RateRepository => throw new NotImplementedException();

        public Task CompleteAsync()
        {
            throw new NotImplementedException();
        }


        public UnitOfWork(DataContext context)
        {
            _context = context;
            _rateRepository = new Lazy<IRepository<Rate>>(() => new Repository<Rate>(_context));
        }
    }
}
