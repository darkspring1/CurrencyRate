using CurrencyRate.Dal.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate.Dal.Ef
{
    public class Repository<T> : IRepository<T>
       where T : class
    {

        class EmptySpecification : ISpecification<T>
        {

            public EmptySpecification()
            {
            }

            public IQueryable<T> Build(IQueryable<T> source)
            {
                return source;
            }
        }

        protected virtual async Task<T> GetOneAsync(Func<IQueryable<T>, Task<T>> getOneFunc, ISpecification<T> specification, bool asNoTracking = false)
        {
            var states = await GetStatesAsync(specification);
            if (asNoTracking)
            {
                states = states.AsNoTracking();
            }
            var state = await getOneFunc(states);

            if (state == null)
            {
                return null;
            }

            return state;
        }

        protected DbContext DbContext { get; }
        protected DbSet<T> CurrentSet { get; }

      

        protected virtual Task<IQueryable<T>> GetStatesAsync(ISpecification<T> specification)
        {
            return Task.FromResult(specification.Build(CurrentSet));
        }

        public Repository(DbContext context)
        {
            CurrentSet = context.Set<T>();
            DbContext = context;
        }

        public void Add(T entity)
        {
            DbContext.Add(entity);
        }

        public virtual void Delete(params T[] entities)
        {
            DbContext.RemoveRange(entities);
        }

        public void Attach(params T[] entities)
        {
            DbContext.AttachRange(entities);
        }

        public Task<T> FirstAsync(ISpecification<T> specification, bool asNoTracking = false)
        {
            return GetOneAsync(states => states.FirstAsync(), specification, asNoTracking);
        }

        public Task<T> FirstAsync(bool asNoTracking = false)
        {
            return FirstAsync(new EmptySpecification(), asNoTracking);
        }

        public Task<T> FirstOrDefaultAsync(ISpecification<T> specification, bool asNoTracking = false)
        {
            return GetOneAsync(states => states.FirstOrDefaultAsync(), specification, asNoTracking);
        }

        public Task<T> FirstOrDefaultAsync(bool asNoTracking = false)
        {
            return FirstOrDefaultAsync(new EmptySpecification(), asNoTracking);
        }

        public Task<T> SingleAsync(ISpecification<T> specification, bool asNoTracking = false)
        {
            return GetOneAsync(states => states.SingleAsync(), specification, asNoTracking);
        }

        public Task<T> SingleOrDefaultAsync(ISpecification<T> specification, bool asNoTracking = false)
        {
            return GetOneAsync(states => states.SingleOrDefaultAsync(), specification, asNoTracking);
        }

        public Task<T> SingleAsync(bool asNoTracking = false)
        {
            return SingleAsync(new EmptySpecification(), asNoTracking);
        }

        public virtual async Task<T[]> GetEntitiesAsync(ISpecification<T> specification, bool asNoTracking = false)
        {
            var states = await GetStatesAsync(specification);
            if (asNoTracking)
            {
                states = states.AsNoTracking();
            }
            var result = states.ToArray();

            return result;
        }

        public Task<T[]> GetEntitiesAsync()
        {
            return GetEntitiesAsync(new EmptySpecification());
        }

        public void Update(T entity)
        {
            DbContext.Update(entity);
        }

        public async Task<bool> AnyAsync(ISpecification<T> specification)
        {
            var states = await GetStatesAsync(specification);
            return await states.AnyAsync();
        }

    }
}
