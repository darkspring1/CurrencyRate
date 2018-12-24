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

    [DisallowConcurrentExecution] //ВАЖНО! запрещаем параллельное выполнение для этого Job'a
    public class RateJob : BaseService, IJob
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly string _jobId;

        void WriteLog(string message)
        {
            Logger.LogInformation($"JobId: {_jobId} {message}");
        }

       
        public RateJob(IServiceProvider serviceProvider) : base(serviceProvider.GetRequiredService<ILogger<RateJob>>())
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
                    var date = DateTime.UtcNow.Date;
                    var cndService = scope.ServiceProvider.GetRequiredService<CnbService>();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                    var ratesResultTask = cndService.GetDailyRatesAsync(date);
                    var entitiesTask = unitOfWork.RateRepository.GetEntitiesAsync(Specifications.DailyRate(date));

                    await Task.WhenAll(ratesResultTask, entitiesTask);

                    if (ratesResultTask.Result.IsFaulted)
                    {
                        return ratesResultTask.Result;
                    }

                    var entitiesFromDb = entitiesTask.Result.ToDictionary(x => x.Code);

                    
                    foreach (var r in ratesResultTask.Result.Data)
                    {
                        Rate rateForUpdate = null;
                        //проверим, если курс уже записан в БД, просто обновим его
                        if (entitiesFromDb.TryGetValue(r.Code, out rateForUpdate))
                        {
                            rateForUpdate.SetValue(r.Value);
                        }
                        else
                        {
                            //если курса нет в БД создадим и добавим его
                            unitOfWork.RateRepository.Add(Rate.Create(r.Code, r.Value, r.Amount, r.Date));
                        }
                    }

                    await unitOfWork.CompleteAsync();
                }
                
                WriteLog("finished.");
                return ServiceResult<CnbError>.Success();
            });
        }
    }
}
