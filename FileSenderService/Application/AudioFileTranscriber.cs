namespace FileSenderService.Application
{
    using FileSenderService.Domain;
    using System.IO;

    public class AudioFileTranscriber
    {
        private readonly ITranscriber _fileTranscriber;
        private readonly IFileSave _fileSaver;

        public AudioFileTranscriber(ITranscriber fileTranscriber, IFileSave fileSaver)
        {
            this._fileTranscriber = fileTranscriber;
            this._fileSaver = fileSaver;
        }

        public void Transcribe(AudioFile file)
        {
            Stream transcribedFile = this._fileTranscriber.Transcribe(Path.Combine(file.Path, file.Name));

            this._fileSaver.Save(Path.Combine(file.Path, file.Name), transcribedFile);
        }
    }
}
