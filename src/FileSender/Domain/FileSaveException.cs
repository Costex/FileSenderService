namespace FileSender.Domain
{
    using System;

    public class FileSaveException : Exception
    {
        public FileSaveException(string nameFile)
        {
            this.Message = $"File {nameFile} couldn't be saved";
        }

        public override string Message { get; }
    }
}
