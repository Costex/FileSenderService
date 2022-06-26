namespace FileSender.Domain
{
    public partial class TranscribeFile
    {
        private const string EXTENSION = ".TXT";

        public static TranscribeFile Transcription(string name, string extension, int size, byte[] content)
        {
            Validate(name, extension, size);

            return new TranscribeFile(name, extension, size, content);
        }

        public static void Validate(string name, string extension, int size)
        {
            if (string.IsNullOrEmpty(extension) || !extension.ToUpper().Equals(EXTENSION))
            {
                throw new FileFormatException(name);
            }

            if (size <= 0)
            {
                throw new FileSizeException(name);
            }
        }
    }
}
