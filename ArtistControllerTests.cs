using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.ArtistService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class ArtistControllerTests
    {
        [Fact]
        public async Task Create_ReturnsBadRequest_WhenRequestIsNull()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ArtistController>>();
            var mockArtistService = new Mock<IArtistService>();
            var controller = new ArtistController(mockLogger.Object, mockArtistService.Object);

            // Act
            var result = await controller.Create(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenNameIsNullOrWhitespace()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ArtistController>>();
            var mockArtistService = new Mock<IArtistService>();
            var controller = new ArtistController(mockLogger.Object, mockArtistService.Object);
            var request = new ArtistRequestModel { Name = null };

            // Act
            var result = await controller.Create(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Create_ReturnsConflict_WhenArtistAlreadyExists()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ArtistController>>();
            var mockArtistService = new Mock<IArtistService>();
            mockArtistService.Setup(service => service.CreateArtist(It.IsAny<string>(), It.IsAny<ArtistRequestModel>())).ThrowsAsync(new ArgumentException("Artist already exists."));
            var controller = new ArtistController(mockLogger.Object, mockArtistService.Object);
            var request = new ArtistRequestModel { Name = "Test Artist" };

            // Act
            var result = await controller.Create(request);

            // Assert
            var conflictResult = Assert.IsType<ConflictObjectResult>(result.Result);
            Assert.Equal("aaa test", conflictResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenIdIsNull()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ArtistController>>();
            var mockArtistService = new Mock<IArtistService>();
            var controller = new ArtistController(mockLogger.Object, mockArtistService.Object);

            // Act
            var result = await controller.GetById(0);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenArtistNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<ArtistController>>();
            var mockArtistService = new Mock<IArtistService>();
            mockArtistService.Setup(service => service.FindById(It.IsAny<int>())).ThrowsAsync(new ArgumentException("Artist not found."));
            var controller = new ArtistController(mockLogger.Object, mockArtistService.Object);

            // Act
            var result = await controller.GetById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Artist not found.", notFoundResult.Value);
        }
    }
}
