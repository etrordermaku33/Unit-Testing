using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.AlbumService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class AlbumControllerTests
    {
        [Fact]
        public async Task CreateAlbum_ReturnsBadRequest_WhenAlbumIsNull()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AlbumController>>();
            var mockAlbumService = new Mock<IAlbumService>();
            var controller = new AlbumController(mockLogger.Object, mockAlbumService.Object);

            // Act
            var result = await controller.CreateAlbum(null);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsBadRequest_WhenIdIsNull()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AlbumController>>();
            var mockAlbumService = new Mock<IAlbumService>();
            var controller = new AlbumController(mockLogger.Object, mockAlbumService.Object);

            // Act
            var result = await controller.GetById(0); // Pass 0 to simulate null ID

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenAlbumNotFound()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<AlbumController>>();
            var mockAlbumService = new Mock<IAlbumService>();
            mockAlbumService.Setup(service => service.FindById(It.IsAny<int>())).ThrowsAsync(new ArgumentException("Album not found."));
            var controller = new AlbumController(mockLogger.Object, mockAlbumService.Object);

            // Act
            var result = await controller.GetById(1); 

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Album not found.", notFoundResult.Value);
        }


    }
}
