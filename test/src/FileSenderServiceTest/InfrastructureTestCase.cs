namespace FileSenderQuartzServiceTest
{
    using FileSenderService.apps.QuartzService;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;

    public sealed class InfrastructureTestCase
    {
        public static IServiceProvider GetServiceProvider()
        {
            Startup startup = null;
            IServiceCollection serviceCollection = null;
            WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddConfiguration(hostingContext.Configuration);
                    config.AddJsonFile("appsettings.json");
                    startup = new Startup(config.Build());
                })
                .ConfigureServices(sc =>
                {
                    startup.ConfigureServices(sc);
                    serviceCollection = sc;
                })
                .UseStartup<EmptyStartup>()
                .Build();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
