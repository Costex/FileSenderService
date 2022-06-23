namespace FileSenderService.Infrastructre.ScheduleConfiguration
{
    using Quartz;
    using Quartz.Impl;
    using System;
    using System.Collections.Generic;

    public class JobScheduler
    {
        private readonly string _cronSchedule;
        private readonly string _filePath;
        private readonly IServiceProvider _serviceProvider;

        public JobScheduler(string cronSchedule, string filePath, IServiceProvider serviceProvider)
        {
            this._cronSchedule = cronSchedule;
            this._filePath = filePath;
            this._serviceProvider = serviceProvider;
        }

        public void Start()
        {

            IScheduler scheduler = (IScheduler)StdSchedulerFactory.GetDefaultScheduler();

            scheduler.Start();

            IDictionary<string, object> dictionary = new Dictionary<string, object>();
            dictionary.Add("filePath", this._filePath);
            dictionary.Add("serviceProvider", this._serviceProvider);

            IJobDetail job = JobBuilder.Create<FileTranscriptionJob>()
                .UsingJobData("filePath", this._filePath)
                .SetJobData(new JobDataMap(dictionary))
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("FileTranscriptionJob", "FileTranscription")
                .WithCronSchedule(this._cronSchedule)
                .StartAt(DateTime.UtcNow)
                .WithPriority(1)
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}
