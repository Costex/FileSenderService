namespace FileSender.Infrastructure
{
    using FileSender.Domain;
    using Microsoft.Extensions.Logging;
    using Polly;
    using System;
    using System.IO;

    public sealed class TranscriberInCloud : InvoxMedicalClient, ITranscriber
    {
        private readonly ILogger<TranscriberInCloud> _log;
        private readonly IAudioFileProvider _audioFileProvider;

        public TranscriberInCloud(ILogger<TranscriberInCloud> log, IAudioFileProvider audioFileProvider)
        {
            this._log = log;
            this._audioFileProvider = audioFileProvider;
        }

        public TranscribeFile Transcribe(AudioFile audioFile)
        {
            Policy policy = Policy
                .Handle<Exception>()
                .Retry(3, (exception, retryCount) =>
                {
                    this._log.LogInformation($"Error file transcription: {audioFile.Name}. Retrycount: {retryCount}");
                });

            PolicyResult<TranscribeFile> policyResult = policy.ExecuteAndCapture(
                () =>
                    {
                        Stream audioFileContent = this._audioFileProvider.GetAudioFileContent(audioFile);
                        Stream transcriptionContent = this.TranscribeFile(audioFileContent);

                        byte[] buffer = new byte[(int)(transcriptionContent.Length)];
                        transcriptionContent.Read(buffer, 0, buffer.Length);

                        TranscribeFile transcribeFile = new TranscribeFile(
                            audioFile.Name,
                            ".txt",
                            buffer.Length,
                            buffer);

                        return transcribeFile;
                    });

            if (policyResult.FinalException != null)
            {
                this._log.LogError(policyResult.FinalException, $"File transcription fail: {audioFile.Name}");
                throw new FileTranscriptException(audioFile.Name);
            }

            return policyResult.Result;
        }
    }
}
