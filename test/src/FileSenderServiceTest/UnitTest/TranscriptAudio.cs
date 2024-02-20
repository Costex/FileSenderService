namespace FileSenderQuartzServiceTest.UnitTest
{
    using FileSender.Application;
    using FileSender.Domain;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Collections.Generic;
    using System.IO;
    using Xunit;

    public sealed class TranscriptAudio
    {
        [Fact]
        public void send_audio_to_transcript()
        {
            // Arrange
            var audioPath = "./Resources/AudioAceptable.mp3";
            var fileInfo = new FileInfo(audioPath);
            var audioFile = AudioFile.CreateFileToSend(
                            Path.GetFileNameWithoutExtension(fileInfo.Name),
                            fileInfo.Extension,
                            (int)fileInfo.Length);
            var transcribeFileDummy = new TranscribeFile("dummy", TranscribeFile.EXTENSION, 5, new byte[] { 0, 0, 0, 0, 0 });

            var log = new Mock<ILogger<AudioFileTranscriber>> ();
            var fileSaver = new Mock<ITranscribeFileSave>();
            var audioFileProvider = new Mock<IAudioFileProvider>();
            audioFileProvider.Setup(x => x.GetAllAudioFiles())
                             .Returns(new List<AudioFile>() { audioFile });
            var fileTranscriber = new Mock<ITranscriber>();
            fileTranscriber.Setup(x => x.Transcribe(audioFile))
                           .Returns(transcribeFileDummy);

            var request = new AudioFileTranscriberRequest() { BlockFiles = 3 };
            var sut = new AudioFileTranscriber(log.Object, audioFileProvider.Object, fileTranscriber.Object, fileSaver.Object);

            // Act
            sut.Transcribe(request);

            // Assert
            audioFileProvider.Verify(x => x.GetAllAudioFiles(), Times.Once);
            fileTranscriber.Verify(x => x.Transcribe(audioFile), Times.Once);
            fileSaver.Verify(x => x.Save(transcribeFileDummy), Times.Once);
        }
    }
}
