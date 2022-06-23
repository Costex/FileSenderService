namespace FileSenderService.Infrastructre.FileSaver
{
    using FileSenderService.Domain;
    using Microsoft.Extensions.Logging;
    using System;
    using System.IO;

    public class FileSaveInDisk : IFileSave
    {
        private readonly ILogger<FileSaveInDisk> _log;

        public FileSaveInDisk(ILogger<FileSaveInDisk> log)
        {
            this._log = log;
        }

        public void Save(string fileName, Stream fileContent)
        {
            string path = Path.GetDirectoryName(fileName) + "\\" + Path.GetFileNameWithoutExtension(fileName) + ".txt";

            try
            {
                using (FileStream fileStream = File.Create(path))
                {
                    fileContent.Seek(0, SeekOrigin.Begin);
                    fileContent.CopyTo(fileStream);
                }
            }
            catch (Exception exe)
            {
                this._log.LogError($"Error cuando se guardaba el archivo en la ruta \"{path}\"", exe);
                throw new FileSaveException(Path.GetFileNameWithoutExtension(fileName));
            }
        }
    }
}
