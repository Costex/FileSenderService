namespace FileSenderServiceTest
{
    using FileSenderService;
    using FileSenderService.Domain;
    using FileSenderService.Infrastructre.CloudTranscriber;
    using FileSenderService.Infrastructre.FileSaver;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    [TestClass]
    public class FileTranscriptionJobTest
    {
        [TestMethod]
        public void SaveFile()
        {
            string testFileEmptyPath = string.Empty;
            string testSaveFilePath = string.Empty;
            using (FileStream file = new FileStream("./TempFileToSave.mp3", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                IFileSave fileSave = (IFileSave)this.InitServiceProvider().GetService(typeof(IFileSave));
                fileSave.Save(file.Name, file);

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

        [TestMethod]
        public void SendFile_NotNullResponse()
        {
            string tempFile = string.Empty;
            using (FileStream file = new FileStream("./TempFileToSend1.mp3", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                tempFile = file.Name;
            }

            ITranscriber transcriber = (ITranscriber)this.InitServiceProvider().GetService(typeof(ITranscriber));
            Stream result = transcriber.Transcribe(tempFile);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void SendFile_ResponseFileWithContent()
        {
            string tempFile = string.Empty;
            using (FileStream file = new FileStream("./TempFileToSend2.mp3", FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                tempFile = file.Name;
            }

            ITranscriber transcriber = (ITranscriber)this.InitServiceProvider().GetService(typeof(ITranscriber));
            Stream result = transcriber.Transcribe(tempFile);

            Assert.IsTrue(result.Length > 0);
            
        }

        private IServiceProvider InitServiceProvider()
        {
            Startup startup = null;
            IServiceCollection serviceCollection = null;
            WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddConfiguration(hostingContext.Configuration);
                    config.AddJsonFile("appsettings.json");
                    startup = new Startup(config.Build());
                })
                .ConfigureServices(sc =>
                {
                    startup.ConfigureServices(sc);
                    serviceCollection = sc;
                })
                .UseStartup<EmptyStartup>()
                .Build();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
