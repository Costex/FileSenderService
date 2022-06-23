namespace FileSenderService.Domain
{
    using System;

    public class FileFormatException : Exception
    {
        public FileFormatException(string nameFile)
        {
            this.Message = $"El archivo {nameFile} no cumple con el formato esperado";
        }

        public override string Message { get; }
    }
}
