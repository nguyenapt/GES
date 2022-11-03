using Quartz;
using Quartz.Impl;

namespace GES.Clients.Web.Helpers
{
    public class JobScheduler
    {
        public static void Start()
        {
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<PreloadJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("preload-trigger01", "group01")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(60).RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);

            //Run job to warm-up application pool
            IJobDetail jobWarmUp = JobBuilder.Create<WarmUpAppPoolJob>().Build();

            ITrigger triggerWarmUp = TriggerBuilder.Create()
            .WithIdentity("warm-up-app-pool-trigger01", "group01")
            .StartNow()
            .WithSimpleSchedule(x => x.WithIntervalInMinutes(18).RepeatForever())
            .Build();

            scheduler.ScheduleJob(jobWarmUp, triggerWarmUp);

        }
    }
}