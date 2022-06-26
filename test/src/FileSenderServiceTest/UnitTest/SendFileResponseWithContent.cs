namespace FileSenderQuartzServiceTest.UnitTest
{
    using FileSender.Domain;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;

    [TestClass]
    public sealed class SendFileResponseWithContent
    {
        [TestMethod]
        public void SendFile()
        {
            AudioFile audioFile = null;
            using (FileStream file = new FileStream("./Resources/AudioAceptable.mp3", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                audioFile = AudioFile.Send("AudioAceptable", ".mp3", (int)file.Length);
            }

            var serviceProvider = InfrastructureTestCase.GetServiceProvider();
            ITranscriber transcriber = (ITranscriber)serviceProvider.GetService(typeof(ITranscriber));

            TranscribeFile transcribeFile = transcriber.Transcribe(audioFile);

            Assert.IsTrue(transcribeFile.Size > 0);
        }
    }
}
