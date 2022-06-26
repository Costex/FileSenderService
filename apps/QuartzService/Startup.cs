namespace FileSenderService.apps.QuartzService
{
    using FileSenderService.apps.QuartzService.Extension;
    using FileSenderService.apps.QuartzService.Extension.DependencyInjection;
    using FileSenderService.apps.QuartzService.Jobs;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Quartz;
    using Serilog;

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
            services.AddApplication();
            services.AddInfrastructure(this.Configuration);

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
