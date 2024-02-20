namespace FileSenderQuartzServiceTest.UnitTest
{
    using FileSender.Domain;
    using System;
    using System.IO;
    using Xunit;

    public sealed class AudioFileBusinessRules
    {

        [Theory]
        [InlineData("../Resources/AceptableAudio.mp3", AudioFile.MAX_SIZE - 5)]
        [InlineData("../Resources/CorrectAudio.mp3", AudioFile.MIN_SIZE + 5)]
        public void create_audio_file(string audioPath, int length)
        {
            // Arrange
            var fileInfo = new FileInfo(audioPath);

            // Act
            var sut = AudioFile.CreateFileToSend(
                            Path.GetFileNameWithoutExtension(fileInfo.Name),
                            fileInfo.Extension,
                            length);

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<AudioFile>(sut);
        }

        [Theory]
        [InlineData("../Resources/IncorrectFormatAudio.wma", AudioFile.MAX_SIZE - 5)]
        [InlineData("../Resources/IncorrectFormatAudio.mp4", AudioFile.MIN_SIZE + 5)]
        public void create_audio_with_incorrect_format(string audioPath, int length)
        {
            // Arrange
            var fileInfo = new FileInfo(audioPath);
            Exception sut = null;

            // Act
            try
            {
                var audioFile = AudioFile.CreateFileToSend(
                                Path.GetFileNameWithoutExtension(fileInfo.Name),
                                fileInfo.Extension,
                                length);
            } catch (Exception ex)
            {
                sut = ex;
            }

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<FileFormatException>(sut);
        }
        
        [Theory]
        [InlineData("../Resources/LittleAudio.mp3", AudioFile.MIN_SIZE - 5)]
        [InlineData("../Resources/BigAudio.mp3", AudioFile.MAX_SIZE + 5)]
        public void create_audio_with_incorrect_size(string audioPath, int length)
        {
            // Arrange
            var fileInfo = new FileInfo(audioPath);
            Exception sut = null;

            // Act
            try
            {
                var audioFile = AudioFile.CreateFileToSend(
                                Path.GetFileNameWithoutExtension(fileInfo.Name),
                                fileInfo.Extension,
                                length);
            } catch (Exception ex)
            {
                sut = ex;
            }

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<FileSizeException>(sut);
        }
    }
}
