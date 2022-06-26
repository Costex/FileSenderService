namespace FileSenderService.apps.QuartzService.Extension.DependencyInjection
{
    using FileSender.Application;
    using Microsoft.Extensions.DependencyInjection;

    public static class Application
    {
        internal static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<AudioFileTranscriber, AudioFileTranscriber>();

            return services;
        }
    }
}
