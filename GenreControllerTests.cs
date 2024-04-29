using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.GenreService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class GenreControllerTests
    {
        [Fact]
        public async Task GetGenreById_ReturnsNotFound_WhenGenreNotFound()
        {
            // Arrange
            var mockGenreService = new Mock<IGenreService>();
            mockGenreService.Setup(service => service.FindById(It.IsAny<int>())).ThrowsAsync(new ArgumentException("Genre not found."));
            var controller = new GenreController(mockGenreService.Object);

            // Act
            var result = await controller.GetGenreById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Genre not found.", notFoundResult.Value);
        }

        [Fact]
        public async Task GetAllGenres_ReturnsOk_WithGenres()
        {
            // Arrange
            var mockGenreService = new Mock<IGenreService>();
            var genres = new List<GenreResponseModel>
            {
                new GenreResponseModel { GenreId = 1, Name = "Rock" },
                new GenreResponseModel { GenreId = 2, Name = "Pop" }
            };
            mockGenreService.Setup(service => service.GetAll()).ReturnsAsync(genres);
            var controller = new GenreController(mockGenreService.Object);

            // Act
            var result = await controller.GetAllGenres();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<GenreResponseModel>>(okResult.Value);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task CreateGenre_ReturnsCreatedAtAction_WithGenre()
        {
            // Arrange
            var mockGenreService = new Mock<IGenreService>();
            var genreRequest = new GenreRequestModel { Name = "Jazz" };
            var createdGenre = new GenreResponseModel { GenreId = 1, Name = "Jazz" };
            mockGenreService.Setup(service => service.CreateGenre(genreRequest)).ReturnsAsync(createdGenre);
            var controller = new GenreController(mockGenreService.Object);

            // Act
            var result = await controller.CreateGenre(genreRequest);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var model = Assert.IsAssignableFrom<GenreResponseModel>(createdAtActionResult.Value);
            Assert.Equal("Jazz", model.Name);
        }

        [Fact]
        public async Task DeleteGenreById_ReturnsNotFound_WhenGenreNotFound()
        {
            // Arrange
            var mockGenreService = new Mock<IGenreService>();
            mockGenreService.Setup(service => service.DeleteById(It.IsAny<int>())).ThrowsAsync(new ArgumentException("Genre not found."));
            var controller = new GenreController(mockGenreService.Object);

            // Act
            var result = await controller.DeleteGenreById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Genre not found.", notFoundResult.Value);
        }
    }
}
