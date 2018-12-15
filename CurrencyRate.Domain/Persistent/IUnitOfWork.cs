using CurrencyRate.Dal.Abstractions;
using CurrencyRate.Domain.Entities;
using System.Threading.Tasks;

namespace CurrencyRate.Domain.Persistent
{
    public interface IUnitOfWork
    {
        IRepository<Rate> RateRepository { get; }

        Task CompleteAsync();
    }
}
