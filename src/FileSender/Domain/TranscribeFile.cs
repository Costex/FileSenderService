namespace FileSender.Domain
{
    public partial class TranscribeFile
    {
        public TranscribeFile(string name, string extension, int size, byte[] content)
        {
            this.Name = name;
            this.Extension = extension;
            this.Size = size;
            this.Content = content;
        }

        public string Name { get; }

        public string Extension { get; }

        public int Size { get; }

        public byte[] Content { get; }
    }
}
