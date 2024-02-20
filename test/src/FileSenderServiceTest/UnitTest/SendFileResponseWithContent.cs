namespace FileSenderQuartzServiceTest.UnitTest
{
    using FileSender.Domain;
    using System.IO;

    public sealed class SendFileResponseWithContent
    {
        public void SendFile()
        {
            AudioFile audioFile = null;
            using (FileStream file = new FileStream("./Resources/AudioAceptable.mp3", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                audioFile = AudioFile.CreateFileToSend("AudioAceptable", ".mp3", (int)file.Length);
            }

            var serviceProvider = InfrastructureTestCase.GetServiceProvider();
            ITranscriber transcriber = (ITranscriber)serviceProvider.GetService(typeof(ITranscriber));

            TranscribeFile transcribeFile = transcriber.Transcribe(audioFile);

        }
    }
}
