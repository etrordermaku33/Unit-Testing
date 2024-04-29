using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.SearchService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class SearchControllerTests
    {
        [Fact]
        public async Task SearchSongs_ReturnsOk_WithSongs()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            var songs = new List<SongResponseModel>
            {
                new SongResponseModel { SongId = 1, Title = "Song 1" },
                new SongResponseModel { SongId = 2, Title = "Song 2" }
            };
            mockSearchService.Setup(service => service.SearchSongs(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(songs);
            var controller = new SearchController(mockSearchService.Object);

            // Act
            var result = await controller.SearchSongs("query");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<SongResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task SearchArtists_ReturnsOk_WithArtists()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            var artists = new List<ArtistResponseModel>
            {
                new ArtistResponseModel { ArtistId = 1, Name = "Artist 1" },
                new ArtistResponseModel { ArtistId = 2, Name = "Artist 2" }
            };
            mockSearchService.Setup(service => service.SearchArtists(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(artists);
            var controller = new SearchController(mockSearchService.Object);

            // Act
            var result = await controller.SearchArtists("query");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<ArtistResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task SearchAlbums_ReturnsOk_WithAlbums()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            var albums = new List<AlbumResponseModel>
            {
                new AlbumResponseModel { AlbumId = 1, Title = "Album 1" },
                new AlbumResponseModel { AlbumId = 2, Title = "Album 2" }
            };
            mockSearchService.Setup(service => service.SearchAlbums(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(albums);
            var controller = new SearchController(mockSearchService.Object);

            // Act
            var result = await controller.SearchAlbums("query");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<AlbumResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task SearchUsers_ReturnsOk_WithUsers()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            var users = new List<UserResponseModel>
            {
                new UserResponseModel { UserId = 1, Username = "User 1" },
                new UserResponseModel { UserId = 2, Username = "User 2" }
            };
            mockSearchService.Setup(service => service.SearchUsers(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(users);
            var controller = new SearchController(mockSearchService.Object);

            // Act
            var result = await controller.SearchUsers("query");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<UserResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task SearchPublicPlaylist_ReturnsOk_WithPlaylists()
        {
            // Arrange
            var mockSearchService = new Mock<ISearchService>();
            var playlists = new List<PlaylistResponseModel>
            {
                new PlaylistResponseModel { PlaylistId = 1, Name = "Playlist 1" },
                new PlaylistResponseModel { PlaylistId = 2, Name = "Playlist 2" }
            };
            mockSearchService.Setup(service => service.SearchPublicPlaylist(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(playlists);
            var controller = new SearchController(mockSearchService.Object);

            // Act
            var result = await controller.SearchPublicPlaylist("query");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<PlaylistResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }
    }
}
