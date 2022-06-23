namespace FileSenderService.Domain
{
    public sealed class AudioFile
    {
        // Tamaño minimo de 50KB
        private const int MIN_SIZE = 51200;

        // Tamaño maximo de 3MB
        private const int MAX_SIZE = 3145728;

        private const string EXTENSION = ".MP3";

        public AudioFile(string name, string path, string extension, int size)
        {
            this.Validate(name, extension, size);

            this.Name = name;
            this.Path = path;
            this.Size = size;
        }

        public string Name { get; }

        public string Path { get; }

        public int Size { get; }

        private void Validate(string name, string extension, int size)
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
