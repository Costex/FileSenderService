namespace FileSenderService.Domain
{
    using System;

    public class FileSaveException : Exception
    {
        public FileSaveException(string nameFile)
        {
            this.Message = $"El archivo {nameFile} no se ha podido guardar";
        }

        public override string Message { get; }
    }
}
