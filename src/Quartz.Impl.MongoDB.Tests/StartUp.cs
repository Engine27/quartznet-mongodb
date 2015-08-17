using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;

namespace Quartz.Impl.MongoDB.Tests
{
    public class StartUp
    {
        public static void Main(String[] args)
        {
            NameValueCollection properties = new NameValueCollection();
            properties["quartz.scheduler.instanceName"] = "JobStoreTest";
            properties["quartz.scheduler.instanceId"] = System.Environment.MachineName + DateTime.UtcNow.Ticks;
            properties["quartz.jobStore.type"] = "Quartz.Impl.MongoDB.JobStore, Quartz.Impl.MongoDB";
            IScheduler scheduler = new Quartz.Impl.StdSchedulerFactory(properties).GetScheduler();
            scheduler.Clear();

            // and start it off
            scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>().WithIdentity("job1", "group1").Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow().WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever()).Build();

            // Tell quartz to schedule the job using our trigger
            scheduler.ScheduleJob(job, trigger);

            //scheduler.PauseAll();
            //Assert.IsFalse(scheduler.IsJobGroupPaused("group1"));
            //Assert.IsTrue(scheduler.IsTriggerGroupPaused("group1"));

            //scheduler.ResumeAll();
            //Assert.IsFalse(scheduler.IsJobGroupPaused("group1"));
            //Assert.IsFalse(scheduler.IsTriggerGroupPaused("group1"));

            //scheduler.ListenerManager.AddJobListener(new DummyJobListener(new ObservableCollection<JobKey>(), new ObservableCollection<JobKey>(), new ObservableCollection<JobKey>()), KeyMatcher<JobKey>.KeyEquals(job.Key));


            // some sleep to show what's happening
            Thread.Sleep(TimeSpan.FromSeconds(10));

            // and last shut down the scheduler when you are ready to close your program
            scheduler.Shutdown();

            Console.ReadLine();
        }
    }

    public class HelloJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("Greetings from HelloJob!");
        }
    }

}
