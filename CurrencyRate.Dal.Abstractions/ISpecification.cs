using System.Linq;

namespace CurrencyRate.Dal.Abstractions
{

    public interface ISpecification<TState>
    {
        IQueryable<TState> Build(IQueryable<TState> source);
    }
}
