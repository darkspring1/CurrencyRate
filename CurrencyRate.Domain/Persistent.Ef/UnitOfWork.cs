﻿using System;
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

        public IRepository<Rate> RateRepository => _rateRepository.Value;

        public Task CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }


        public UnitOfWork(DataContext context)
        {
            _context = context;
            _rateRepository = new Lazy<IRepository<Rate>>(() => new Repository<Rate>(_context));
        }
    }
}
