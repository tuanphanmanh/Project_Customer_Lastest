using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace LSP.Models
{
    public class JobScheduler
    {
        public static async Task Start()
        {

            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler scheduler = await sf.GetScheduler();

            IJobDetail job = JobBuilder.Create<EmailJob>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("trigger1", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInSeconds(30)
            .RepeatForever())
            .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);

            // Start up the scheduler
            await scheduler.Start();
    
        }  
    }
}