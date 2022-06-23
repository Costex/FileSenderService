namespace FileSenderService.Infrastructre.ScheduleConfiguration
{
    using FileSenderService.Application;
    using FileSenderService.Domain;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Quartz;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class FileTranscriptionJob : IJob
    {
        private readonly ILogger<FileTranscriptionJob> _log;
        private readonly IServiceProvider _serviceProvider;
        private readonly string _filePath;

        public FileTranscriptionJob(ILogger<FileTranscriptionJob> log, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            this._log = log;
            this._serviceProvider = serviceProvider;
            this._filePath = configuration.GetSection("FileTranscriptionJob").GetSection("FilePath").Value;
        }

        public Task Execute(IJobExecutionContext context)
        {
            string[] files = Directory.GetFiles(this._filePath);

            ParallelOptions option = new ParallelOptions();
            option.MaxDegreeOfParallelism = 3;

            this._log.LogInformation("Se procede a recorrer los ficheros de 3 en 3");

            Parallel.ForEach(
                files,
                option,
                filePath =>
                {
                    try
                    {
                        FileInfo fileInfo = new FileInfo(filePath);

                        AudioFile audioFile = new AudioFile(
                                                        fileInfo.Name,
                                                        fileInfo.DirectoryName,
                                                        fileInfo.Extension,
                                                        (int)fileInfo.Length);

                        AudioFileTranscriber audioFileTranscriber = (AudioFileTranscriber)this._serviceProvider.GetService(typeof(AudioFileTranscriber));
                        audioFileTranscriber.Transcribe(audioFile);
                    }
                    catch (Exception exception)
                    {
                        this._log.LogError(exception, "Error durante la ejecucion de la tarea de transcripcion.");
                    }
                });

            return Task.CompletedTask;
        }
    }
}
