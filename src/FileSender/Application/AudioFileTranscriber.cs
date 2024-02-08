namespace FileSender.Application
{
    using FileSender.Domain;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class AudioFileTranscriber
    {
        private readonly IAudioFileProvider _audioFileProvider;
        private readonly ITranscriber _fileTranscriber;
        private readonly ITranscribeFileSave _fileSaver;
        private readonly ILogger<AudioFileTranscriber> _log;

        public AudioFileTranscriber(
            ILogger<AudioFileTranscriber> log,
            IAudioFileProvider audioFileProvider,
            ITranscriber fileTranscriber,
            ITranscribeFileSave fileSaver)
        {
            this._log = log;
            this._audioFileProvider = audioFileProvider;
            this._fileTranscriber = fileTranscriber;
            this._fileSaver = fileSaver;
        }

        public void Transcribe(AudioFileTranscriberRequest request)
        {
            var parallelOptions = new ParallelOptions();
            parallelOptions.MaxDegreeOfParallelism = request.BlockFiles;

            var audioFiles = this._audioFileProvider.GetAllAudioFiles();

            Parallel.ForEach(
                audioFiles,
                parallelOptions,
                audioFile =>
                {
                    try
                    {
                        var transcribeFile = this._fileTranscriber.Transcribe(audioFile);

                        this._fileSaver.Save(transcribeFile);
                    }
                    catch (Exception exception)
                    {
                        this._log.LogError(exception, "Error in transcription execution.");
                    }
                });
        }
    }
}
