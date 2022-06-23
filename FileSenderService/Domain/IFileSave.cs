namespace FileSenderService.Domain
{
    using System.IO;

    public interface IFileSave
    {
        void Save(string fileName, Stream fileContent);
    }
}
