using CurrencyRate.HistoricalDataService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace CurrencyRate.DailyRateService
{
    public class ConsoleAppHost
    {
        private readonly ServiceProvider _serviceProvider;
        
        private readonly Settings _settings;
        private readonly ILogger<ConsoleAppHost> _logger;
        private IScheduler _scheduler;

        public ConsoleAppHost(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _settings = serviceProvider.GetRequiredService<Settings>();
            _logger = _serviceProvider.GetRequiredService<ILogger<ConsoleAppHost>>();
        }

        public async Task StopAsync()
        {
            if (_scheduler != null && _scheduler.IsStarted)
            {
                await _scheduler.Shutdown();
            }
        }

        public async Task RunAsync()
        {
            //Запуск раписания
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            _scheduler = await schedFact.GetScheduler();
            _scheduler.JobFactory = new JobFactory(_serviceProvider);

            var jobDetail = JobBuilder.Create<RateJob>().Build();
            var trigger = TriggerBuilder.Create().WithSimpleSchedule(s => s.WithIntervalInSeconds(5).RepeatForever()).Build();
            await _scheduler.ScheduleJob(jobDetail, trigger);
            await _scheduler.Start();
        }


    }
}
