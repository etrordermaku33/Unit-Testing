using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.PlayHistoryService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class PlayHistoryControllerTests
    {
        [Fact]
        public async Task GetPlayHistory_ReturnsNotFound_WhenNoHistoryFound()
        {
            // Arrange
            var mockPlayHistoryService = new Mock<IPlayHistoryService>();
            mockPlayHistoryService.Setup(service => service.GetPlayHistoryByUserId(It.IsAny<string>())).ReturnsAsync(new List<PlayHistoryResponseModel>());
            var controller = new PlayHistoryController(mockPlayHistoryService.Object);

            // Act
            var result = await controller.GetPlayHistory();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("No play history found for the user.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetPlayHistory_ReturnsOk_WithPlayHistory()
        {
            // Arrange
            var mockPlayHistoryService = new Mock<IPlayHistoryService>();
            var playHistory = new List<PlayHistoryResponseModel>
            {
                new PlayHistoryResponseModel { UserId = 1, SongId = 1, Timestamp = DateTime.UtcNow }
            };
            mockPlayHistoryService.Setup(service => service.GetPlayHistoryByUserId(It.IsAny<string>())).ReturnsAsync(playHistory);
            var controller = new PlayHistoryController(mockPlayHistoryService.Object);

            // Act
            var result = await controller.GetPlayHistory();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<PlayHistoryResponseModel>>(okResult.Value);
            Assert.Single(model);
        }

        [Fact]
        public async Task AddPlayHistory_ReturnsOk_WhenHistoryAdded()
        {
            // Arrange
            var mockPlayHistoryService = new Mock<IPlayHistoryService>();
            var controller = new PlayHistoryController(mockPlayHistoryService.Object);
            var request = new PlayHistoryRequestModel { SongId = 1 };

            // Act
            var result = await controller.AddPlayHistory(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task AddPlayHistory_ReturnsNotFound_WhenSongNotFound()
        {
            // Arrange
            var mockPlayHistoryService = new Mock<IPlayHistoryService>();
            mockPlayHistoryService.Setup(service => service.AddPlayHistory(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<DateTime>())).ThrowsAsync(new ArgumentException("Song not found."));
            var controller = new PlayHistoryController(mockPlayHistoryService.Object);
            var request = new PlayHistoryRequestModel { SongId = 1 };

            // Act
            var result = await controller.AddPlayHistory(request);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal("Song not found.", notFoundResult.Value);
        }
    }
}
