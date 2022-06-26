namespace FileSender.Domain
{
    public partial class AudioFile
    {
        // Tamaño minimo de 50KB
        public const int MIN_SIZE = 51200;

        // Tamaño maximo de 3MB
        public const int MAX_SIZE = 3145728;

        public const string EXTENSION = ".MP3";

        public static AudioFile Send(string name, string extension, int size)
        {
            Validate(name, extension, size);

            return new AudioFile(name, extension, size);
        }

        private static void Validate(string name, string extension, int size)
        {
            if (!extension.ToUpper().Equals(EXTENSION))
            {
                throw new FileFormatException(name);
            }

            if (size < MIN_SIZE || MAX_SIZE < size)
            {
                throw new FileSizeException(name);
            }
        }
    }
}
