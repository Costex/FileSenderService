namespace FileSender.Infrastructure
{
    using System.IO;

    public class InvoxMedicalClient
    {
        protected Stream TranscribeFile(Stream fileContent)
        {
            return new FileStream("./Resources/Prueba1.txt", FileMode.Open, FileAccess.Read);
        }
    }
}
