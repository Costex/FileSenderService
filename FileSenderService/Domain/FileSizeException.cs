namespace FileSenderService.Domain
{
    using System;

    public class FileSizeException : Exception
    {
        public FileSizeException(string nameFile)
        {
            this.Message = $"El archivo {nameFile} no cumple con el tamaño esperado";
        }

        public override string Message { get; }
    }
}
