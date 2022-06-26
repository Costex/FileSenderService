namespace FileSender.Domain
{
    public interface ITranscriber
    {
        TranscribeFile Transcribe(AudioFile audioFile);
    }
}
