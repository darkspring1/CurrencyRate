using CurrencyRate.Domain.Errors;
using System.Linq;

namespace CurrencyRate.Api.Controllers
{
    public class FaultedResultWrap
    {
        public FaultedResultWrap(Error[] errors)
        {
            Errors = errors.Select(e => new ErrorWrap(e)).ToArray();
        }

        public ErrorWrap[] Errors { get; }
    }
}
