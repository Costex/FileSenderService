namespace FileSenderService.Infrastructre.CloudTranscriber
{
    using FileSenderService.Domain;
    using Microsoft.Extensions.Logging;
    using Polly;
    using System;
    using System.IO;

    public sealed class TranscriberInCloud : InvoxMedicalClient, ITranscriber
    {
        private readonly ILogger<TranscriberInCloud> _log;

        public TranscriberInCloud(ILogger<TranscriberInCloud> log)
        {
            this._log = log;
        }

        public Stream Transcribe(string filePath)
        {
            Policy policy = Policy
                .Handle<Exception>()
                .Retry(3, (exception, retryCount) =>
                {
                    this._log.LogInformation($"Error al transcribir el archivo {Path.GetFileName(filePath)}, intento {retryCount}");
                });

            PolicyResult<Stream> policyResult = policy.ExecuteAndCapture(
                () =>
                    {
                        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                        return this.TranscribeFile(fileStream);
                    });

            if (policyResult.FinalException != null)
            {
                this._log.LogError(policyResult.FinalException, $"La transcripción del archivo {Path.GetFileName(filePath)} ha fallado.");
                throw new FileTranscriptException(Path.GetFileName(filePath));
            }

            return policyResult.Result;
        }
    }
}
