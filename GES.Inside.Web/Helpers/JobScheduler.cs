using System;
using System.Collections.Generic;
using GES.Inside.Web.Configs;
using Quartz;
using Quartz.Impl;

namespace GES.Inside.Web.Helpers
{
    public class JobScheduler
    {
        public static void Start()
        {
            var scheduler = StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            // Generate KeyWords Job
            var generateKeyWordsJob = InitGenerateKeyWordsJob();

            scheduler.ScheduleJob(generateKeyWordsJob.Key, generateKeyWordsJob.Value);
        }

        public static KeyValuePair<IJobDetail, ITrigger> InitGenerateKeyWordsJob()
        {
            var job = JobBuilder.Create<GenerateKeywordsJob>().Build();

            // calculate start time
            var startTime = DateTime.UtcNow.AddMinutes(SiteSettings.JobGenerateKeywordsDelayedStartInMinutes);

            var trigger = TriggerBuilder.Create()
                .WithIdentity("generateKeywords-trigger01", "group01")
                .StartAt(new DateTimeOffset(startTime))
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(SiteSettings.JobGenerateKeywordsMinutesInterval).RepeatForever())
                .Build();

            return new KeyValuePair<IJobDetail, ITrigger>(job, trigger);
        }
    }
}