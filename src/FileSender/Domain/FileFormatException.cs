namespace FileSender.Domain
{
    using System;

    public class FileFormatException : Exception
    {
        public FileFormatException(string nameFile)
        {
            this.Message = $"The file {nameFile} doesn't match with the expected format";
        }

        public override string Message { get; }
    }
}
