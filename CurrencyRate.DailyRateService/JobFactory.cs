using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;

namespace CurrencyRate.HistoricalDataService
{
    class JobFactory : SimpleJobFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;

        public JobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = serviceProvider.GetService<ILogger<JobFactory>>();
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var job = (IJob)_serviceProvider.GetService(bundle.JobDetail.JobType);
                if (job == null)
                {
                    throw new Exception($"Unknow job type: '{bundle.JobDetail.JobType.FullName}'");
                }
                return job;
            }
            catch (Exception e)
            {
                _logger.LogError($"Problem while instantiating job '{bundle.JobDetail.Key}' from the {nameof(JobFactory)}.", e);
                throw e;
            }
        }
    }
}
