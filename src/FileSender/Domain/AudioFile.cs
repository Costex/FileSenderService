namespace FileSender.Domain
{
    public partial class AudioFile
    {
        protected AudioFile(string name, string extension, int size)
        {
            this.Name = name;
            this.Extension = extension;
            this.Size = size;
        }

        public string Name { get; }

        public string Extension { get; }

        public int Size { get; }
    }
}
