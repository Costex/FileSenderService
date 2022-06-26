namespace FileSender.Domain
{
    using System.Collections.Generic;
    using System.IO;

    public interface IAudioFileProvider
    {
        List<AudioFile> GetAllAudioFiles();

        Stream GetAudioFileContent(AudioFile audioFile);
    }
}
