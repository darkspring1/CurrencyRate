using CurrencyRate.Abstractions;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Errors;
using CurrencyRate.Domain.Persistent;
using CurrencyRate.Domain.Persistent.Specifications;
using Microsoft.Extensions.Logging;
using System;
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

        public Task<IServiceResult<Error, Rate[]>> GetRatesAsync(int year, int month, string[] currencyCodes)
        {
            return RunAsync(async () =>
            {
                if (year < 1990 || year > DateTime.UtcNow.Year)
                {
                    return ServiceResult<Error, Rate[]>.Fault(null, BlErrors.Error1003(nameof(year), year.ToString()));
                }

                if (month < 1 || month > 12)
                {
                    return ServiceResult<Error, Rate[]>.Fault(null, BlErrors.Error1003(nameof(month), month.ToString()));
                }

                var rates = await _unitOfWork.RateRepository.GetEntitiesAsync(Specifications.MonthRate(year, month, currencyCodes));
                return ServiceResult<Error>.Success(rates.ToArray());
            });
        }
    }
}
