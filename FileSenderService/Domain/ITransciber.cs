namespace FileSenderService.Domain
{
    using System.IO;

    public interface ITranscriber
    {
        Stream Transcribe(string filePath);
    }
}
