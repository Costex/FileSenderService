namespace FileSender.Application
{
    public sealed class AudioFileTranscriberRequest
    {
        /// <summary>
        /// Number of files to send in the same block
        /// </summary>
        public int BlockFiles { get; set; }
    }
}
