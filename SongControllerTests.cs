using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.SongService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class SongControllerTests
    {
        [Fact]
        public async Task GetSong_ReturnsOk_WithSong()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<SongController>>();
            var mockSongService = new Mock<ISongService>();
            var song = new SongResponseModel { SongId = 1, Title = "Song 1" };
            mockSongService.Setup(service => service.FindById(It.IsAny<int>())).ReturnsAsync(song);
            var controller = new SongController(mockLogger.Object, mockSongService.Object);

            // Act
            var result = await controller.GetSong(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<SongResponseModel>(okResult.Value);
            Assert.Equal(1, model.SongId);
            Assert.Equal("Song 1", model.Title);
        }

        [Fact]
        public async Task GetNewSongs_ReturnsOk_WithSongs()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<SongController>>();
            var mockSongService = new Mock<ISongService>();
            var songs = new List<SongResponseModel>
            {
                new SongResponseModel { SongId = 1, Title = "Song 1" },
                new SongResponseModel { SongId = 2, Title = "Song 2" }
            };
            mockSongService.Setup(service => service.GetNew(It.IsAny<int>())).ReturnsAsync(songs);
            var controller = new SongController(mockLogger.Object, mockSongService.Object);

            // Act
            var result = await controller.GetNewSongs(10);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<SongResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }
    }
}
