using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.PlaylistService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class PlaylistControllerTests
    {
        [Fact]
        public async Task GetAllVisiblePlaylists_ReturnsOk_WithPlaylists()
        {
            // Arrange
            var mockPlaylistService = new Mock<IPlaylistService>();
            var playlists = new List<PlaylistResponseModel>
            {
                new PlaylistResponseModel { PlaylistId = 1, Name = "Rock Hits" },
                new PlaylistResponseModel { PlaylistId = 2, Name = "Pop Favorites" }
            };
            mockPlaylistService.Setup(service => service.GetAllVisible(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(playlists);
            var controller = new PlaylistController(mockPlaylistService.Object);

            // Act
            var result = await controller.GetAllVisiblePlaylists();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<PlaylistResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task GetPlaylistById_ReturnsOk_WithPlaylist()
        {
            // Arrange
            var mockPlaylistService = new Mock<IPlaylistService>();
            var playlist = new PlaylistResponseModel { PlaylistId = 1, Name = "Rock Hits" };
            mockPlaylistService.Setup(service => service.FindById(It.IsAny<int>())).ReturnsAsync(playlist);
            var controller = new PlaylistController(mockPlaylistService.Object);

            // Act
            var result = await controller.GetPlaylistById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<PlaylistResponseModel>(okResult.Value);
            Assert.Equal(1, model.PlaylistId);
            Assert.Equal("Rock Hits", model.Name);
        }

        [Fact]
        public async Task CreatePlaylist_ReturnsCreatedAtAction_WithPlaylist()
        {
            // Arrange
            var mockPlaylistService = new Mock<IPlaylistService>();
            var request = new PlaylistRequestModel { Name = "Jazz Classics" };
            var createdPlaylist = new PlaylistResponseModel { PlaylistId = 1, Name = "Jazz Classics" };
            mockPlaylistService.Setup(service => service.CreatePlaylist(request)).ReturnsAsync(createdPlaylist);
            var controller = new PlaylistController(mockPlaylistService.Object);

            // Act
            var result = await controller.CreatePlaylist(request);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsType<PlaylistResponseModel>(createdAtActionResult.Value);
            Assert.Equal(1, model.PlaylistId);
            Assert.Equal("Jazz Classics", model.Name);
        }

        [Fact]
        public async Task DeletePlaylist_ReturnsOk_WithDeletedPlaylist()
        {
            // Arrange
            var mockPlaylistService = new Mock<IPlaylistService>();
            var deletedPlaylist = new PlaylistResponseModel { PlaylistId = 1, Name = "Jazz Classics" };
            mockPlaylistService.Setup(service => service.DeleteById(It.IsAny<int>())).ReturnsAsync(deletedPlaylist);
            var controller = new PlaylistController(mockPlaylistService.Object);

            // Act
            var result = await controller.DeletePlaylist(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<PlaylistResponseModel>(okResult.Value);
            Assert.Equal(1, model.PlaylistId);
            Assert.Equal("Jazz Classics", model.Name);
        }
    }
}
