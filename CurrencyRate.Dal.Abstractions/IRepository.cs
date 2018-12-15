using System.Threading.Tasks;

namespace CurrencyRate.Dal.Abstractions
{
    public interface IRepository<T>
    where T : class
    {
        void Attach(params T[] entities);

        void Update(T entity);

        void Add(T entity);

        void Delete(params T[] entities);

        Task<bool> AnyAsync(ISpecification<T> specification);

        
        Task<T[]> GetEntitiesAsync(ISpecification<T> specification, bool asNoTracking = false);

        Task<T[]> GetEntitiesAsync();

        Task<T> FirstAsync(bool asNoTracking = false);

        Task<T> FirstAsync(ISpecification<T> specification, bool asNoTracking = false);

        Task<T> FirstOrDefaultAsync(ISpecification<T> specification, bool asNoTracking = false);

        Task<T> FirstOrDefaultAsync(bool asNoTracking = false);

        Task<T> SingleAsync(ISpecification<T> specification, bool asNoTracking = false);

        Task<T> SingleOrDefaultAsync(ISpecification<T> specification, bool asNoTracking = false);

        Task<T> SingleAsync(bool asNoTracking = false);
    }
}
