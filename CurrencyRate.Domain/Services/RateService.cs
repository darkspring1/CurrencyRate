using CurrencyRate.Abstractions;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Errors;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Specifications;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate.Domain.Services
{
    public class RateService : BaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RateService(IUnitOfWork unitOfWork, ILogger<RateService> logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<IServiceResult<Error, Rate[]>> GetRatesAsync(int year, int month)
        {
            return RunAsync(async () =>
            {
                var rates = await _unitOfWork.RateRepository.GetEntitiesAsync(Specifications.MonthRate(year, month));
                return ServiceResult<Error>.Success(rates.ToArray());
            });
        }
    }
}
