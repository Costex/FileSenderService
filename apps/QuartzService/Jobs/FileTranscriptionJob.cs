namespace FileSenderService.apps.QuartzService.Jobs
{
    using FileSender.Application;
    using Microsoft.Extensions.Logging;
    using Quartz;
    using System.Threading.Tasks;

    public class FileTranscriptionJob : IJob
    {
        private readonly ILogger<FileTranscriptionJob> _log;
        private readonly AudioFileTranscriber _audioFileTranscriber;

        public FileTranscriptionJob(ILogger<FileTranscriptionJob> log, AudioFileTranscriber audioFileTranscriber)
        {
            this._log = log;
            this._audioFileTranscriber = audioFileTranscriber;
        }

        public Task Execute(IJobExecutionContext context)
        {
            AudioFileTranscriberRequest request = new AudioFileTranscriberRequest
            {
                BlockFiles = 3
            };

            this._log.LogInformation("Starting FileTranscriptionJob.");
            this._audioFileTranscriber.Transcribe(request);
            this._log.LogInformation("FileTranscriptionJob completed.");

            return Task.CompletedTask;
        }
    }
}
