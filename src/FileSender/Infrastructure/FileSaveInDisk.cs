namespace FileSender.Infrastructure
{
    using FileSender.Domain;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    public class FileSaveInDisk : ITranscribeFileSave
    {
        private readonly ILogger<FileSaveInDisk> _log;
        private readonly string _audioFilePath;

        public FileSaveInDisk(ILogger<FileSaveInDisk> log, string audioFilePath)
        {
            this._log = log;
            this._audioFilePath = audioFilePath;
        }

        public void Save(TranscribeFile transcribeFile)
        {
            string path = this._audioFilePath + "\\" + transcribeFile.Name + transcribeFile.Extension;

            try
            {
                using (FileStream fileStream = File.Create(path))
                {
                    fileStream.Write(transcribeFile.Content, 0, transcribeFile.Size);
                }
            }
            catch (Exception exe)
            {
                this._log.LogError($"Error when the file was saved to the path \"{path}\"", exe);
                throw new FileSaveException(transcribeFile.Name);
            }
        }
    }
}
