namespace FileSenderService.apps.QuartzService.Extension.DependencyInjection
{
    using FileSender.Domain;
    using FileSender.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;

    public static class Infrastructure
    {
        internal static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ITranscribeFileSave, FileSaveInDisk>(
                provider => new FileSaveInDisk(
                    provider.GetService<ILogger<FileSaveInDisk>>(),
                    configuration.GetSection("AudioFilePath").Value));

            services.AddScoped<ITranscriber, TranscriberInCloud>();

            services.AddScoped<IAudioFileProvider, AudioFileProvider>(
                provider => new AudioFileProvider(
                    provider.GetService<ILogger<AudioFileProvider>>(),
                    configuration.GetSection("AudioFilePath").Value));

            return services;
        }
    }
}
