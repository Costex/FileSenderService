namespace FileSender.Infrastructure
{
    using FileSender.Domain;
    using Microsoft.Extensions.Logging;
    using System.Collections.Generic;
    using System.IO;

    public class AudioFileProvider : IAudioFileProvider
    {
        private readonly ILogger<AudioFileProvider> _log;
        private readonly string _audioFilePath;

        public AudioFileProvider(ILogger<AudioFileProvider> log, string audioFilePath)
        {
            this._log = log;
            this._audioFilePath = audioFilePath;
        }

        public List<AudioFile> GetAllAudioFiles()
        {
            var audiFiles = new List<AudioFile>();
            string[] files = Directory.GetFiles(this._audioFilePath);
            
            foreach(string filePath in files)
            {
                var fileInfo = new FileInfo(filePath);
                try
                {
                    var audioFile = AudioFile.CreateFileToSend(
                        Path.GetFileNameWithoutExtension(fileInfo.Name),
                        fileInfo.Extension,
                        (int)fileInfo.Length);

                    audiFiles.Add(audioFile);
                }
                catch (FileFormatException formatException)
                {
                    this._log.LogError(formatException.Message);
                    continue;
                }
                catch (FileSizeException sizeException)
                {
                    this._log.LogError(sizeException.Message);
                    continue;
                }
            }

            return audiFiles;
        }

        public Stream GetAudioFileContent(AudioFile audioFile)
        {
            using (var file = new FileStream(Path.Combine(this._audioFilePath, audioFile.Name + audioFile.Extension), FileMode.Open, FileAccess.Read))
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);
                return ms;
            }
        }
    }
}
