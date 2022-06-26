namespace FileSender.Domain
{
    using System;

    public class FileSizeException : Exception
    {
        public FileSizeException(string nameFile)
        {
            this.Message = $"The file {nameFile} doesn't match with the expected size";
        }

        public override string Message { get; }
    }
}
