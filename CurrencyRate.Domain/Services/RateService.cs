using CurrencyRate.Abstractions;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Errors;
using CurrencyRate.Domain.Persistent;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate.Domain.Services
{
    public class RatesService : BaseService
    {
        private readonly IUnitOfWork _unitOfWork;

        public RatesService(IUnitOfWork unitOfWork, ILogger<RateService> logger) : base(logger)
        {
            _unitOfWork = unitOfWork;
        }


        public Task<IServiceResult<Error, Rate[]>> GetRatesAsync()
        {
            return RunAsync(async () =>
            {
                var rates = await _unitOfWork.RateRepository.GetEntitiesAsync();
                return ServiceResult<Error>.Success(rates.ToArray());
            });
        }
    }
}
