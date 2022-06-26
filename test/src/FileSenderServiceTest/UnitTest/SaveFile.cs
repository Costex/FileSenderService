namespace FileSenderQuartzServiceTest.UnitTest
{
    using FileSender.Domain;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.IO;
    using System.Text;

    [TestClass]
    public sealed class SaveFile
    {
        [TestMethod]
        public void Save()
        {
            string testFileEmptyPath = string.Empty;
            string testSaveFilePath = string.Empty;
            using (FileStream file = new FileStream("./Resources/AudioAceptable.mp3", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                var serviceProvider = InfrastructureTestCase.GetServiceProvider();
                ITranscribeFileSave fileSave = (ITranscribeFileSave)serviceProvider.GetService(typeof(ITranscribeFileSave));

                byte[] content = Encoding.ASCII.GetBytes("Prueba");

                TranscribeFile transcribe = TranscribeFile.Transcription(
                    Path.GetFileNameWithoutExtension(file.Name),
                    ".txt",
                    content.Length,
                    content);

                fileSave.Save(transcribe);

                testFileEmptyPath = file.Name;
                testSaveFilePath = Path.GetDirectoryName(file.Name) + '\\' + Path.GetFileNameWithoutExtension(file.Name) + ".txt";
            }

            Assert.IsTrue(File.Exists(testSaveFilePath));

            if (File.Exists(testFileEmptyPath))
            {
                File.Delete(testFileEmptyPath);
            }

            if (File.Exists(testSaveFilePath))
            {
                File.Delete(testSaveFilePath);
            }
        }
    }
}
