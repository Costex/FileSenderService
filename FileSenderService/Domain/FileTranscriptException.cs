namespace FileSenderService.Domain
{
    using System;

    public class FileTranscriptException : Exception
    {
        public FileTranscriptException(string nameFile)
        {
            this.Message = $"La transcripción del archivo {nameFile} ha fallado.";
        }

        public override string Message { get; }
    }
}
