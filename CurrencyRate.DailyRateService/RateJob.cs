using CurrencyRate.Abstractions;
using CurrencyRate.Cnb;
using CurrencyRate.Domain.Entities;
using CurrencyRate.Domain.Persistent;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate.DailyRateService
{
    public class RateJob : BaseService, IJob
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly string _jobId;

        void WriteLog(string message)
        {
            Logger.LogInformation($"JobId: {_jobId} {message}");
        }

       
        public RateJob(IServiceProvider serviceProvider) : base(serviceProvider.GetService<ILogger<RateJob>>())
        {
            _jobId = Guid.NewGuid().ToString();
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            return RunAsync(async () =>
            {
                WriteLog("started.");

                using (var scope = _serviceProvider.CreateScope())
                {
                    var cndService = scope.ServiceProvider.GetRequiredService<CnbService>();

                    var ratesResult = await cndService.GetDailyRatesAsync(DateTime.UtcNow.Date);

                    if (ratesResult.IsFaulted)
                    {
                        return ratesResult;
                    }

                    var rates = ratesResult
                                    .Data
                                    .Select(r => Rate.Create(r.Code, r.Value, r.Date))
                                    .ToArray();

                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    unitOfWork.RateRepository.Add(rates);
                    await unitOfWork.CompleteAsync();
                }
                
                WriteLog("finished.");
                return ServiceResult<CnbError>.Success();
            });
        }
    }
}
