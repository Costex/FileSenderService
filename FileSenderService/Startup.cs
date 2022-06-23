namespace FileSenderService
{
    using FileSenderService.Application;
    using FileSenderService.Domain;
    using FileSenderService.Infrastructre.CloudTranscriber;
    using FileSenderService.Infrastructre.FileSaver;
    using FileSenderService.Infrastructre.ScheduleConfiguration;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Quartz;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            this.ConfigureApplication(services);
            this.ConfigureInfraestructure(services);

            this.ConfigureQuartz(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            loggerFactory.AddFile(this.Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
        }

        private void ConfigureApplication(IServiceCollection services)
        {
            services.AddTransient<AudioFileTranscriber>();
        }

        private void ConfigureInfraestructure(IServiceCollection services)
        {
            services.AddScoped<ITranscriber, TranscriberInCloud>();
            services.AddScoped<IFileSave, FileSaveInDisk>();
        }

        private void ConfigureQuartz(IServiceCollection services)
        {
            services.AddQuartz(quartz => 
            {
                quartz.UseMicrosoftDependencyInjectionJobFactory();
                quartz.AddJobAndTrigger<FileTranscriptionJob>(this.Configuration);
            });

            services.AddQuartzHostedService(quartz => quartz.WaitForJobsToComplete = true);
        }
    }
}
