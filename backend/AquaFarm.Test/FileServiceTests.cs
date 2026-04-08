using AquaFarm.Application.CustomExceptions;
using AquaFarm.Application.Services;
using AquaFarm.Application.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AquaFarm.Test
{
    public class FileServiceTests
    {
        [Fact]
        public async Task UploadImageAsync_ShouldReturnUrl_WhenSuccess()
        {
            // Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService
                .Setup(s => s.UploadImageAsync(It.IsAny<Stream>(), It.IsAny<string?>()))
                .ReturnsAsync("/images/b3ae4ba7-25f7-48a4-a80c-0d1095ec9081.jpg");

            // Act
            var result = await mockFileService.Object.UploadImageAsync(new MemoryStream(), "image.jpg");

            // Assert
            Assert.Equal("/images/b3ae4ba7-25f7-48a4-a80c-0d1095ec9081.jpg", result);
        }

        [Fact]
        public async Task UploadImageAsync_ShouldThrowFileUploadException_WhenError()
        {
            // Arrange
            var mockFileService = new Mock<IFileService>();
            mockFileService
                .Setup(s => s.UploadImageAsync(It.IsAny<Stream>(), It.IsAny<string?>()))
                .ThrowsAsync(new FileUploadException("UPLOAD_IMAGE", 500, "Error upload image"));

            // Act
            var ex = await Assert.ThrowsAsync<FileUploadException>(
                () => mockFileService.Object.UploadImageAsync(new MemoryStream(), "image.jpg")
            );

            // Assert
            Assert.Equal("UPLOAD_IMAGE", ex.Action);
        }
    }
}
