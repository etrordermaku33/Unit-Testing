using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using MusicStreamingService_BackEnd.Controllers;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services;
using MusicStreamingService_BackEnd.Services.AuthService;
using MusicStreamingService_BackEnd.Services.FollowService;
using Xunit;

namespace MusicStreamingService_BackEnd.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task Create_ReturnsOk_WithUser()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserController>>();
            var mockUserService = new Mock<IUserService>();
            var mockAuthService = new Mock<IAuthService>();
            var mockFollowService = new Mock<IFollowService>();
            var request = new UserRequestModel { FullName = "Test User", Username = "testuser", Email = "test@test.com", Password = "password" };
            var user = new UserResponseModel { UserId = 1, FullName = "Test User", Username = "testuser", Email = "test@test.com" };
            mockUserService.Setup(service => service.CreateUser(It.IsAny<UserRequestModel>())).ReturnsAsync(user);
            var controller = new UserController(mockLogger.Object, mockUserService.Object, mockAuthService.Object, mockFollowService.Object);

            // Act
            var result = await controller.Create(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsType<UserResponseModel>(okResult.Value);
            Assert.Equal(1, model.UserId);
            Assert.Equal("Test User", model.FullName);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithToken()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<UserController>>();
            var mockUserService = new Mock<IUserService>();
            var mockAuthService = new Mock<IAuthService>();
            var mockFollowService = new Mock<IFollowService>();
            var request = new LoginRequestModel { Username = "testuser", Password = "password" };
            var token = "test-token";
            var role = "user";
            mockAuthService.Setup(service => service.Authenticate(It.IsAny<LoginRequestModel>())).ReturnsAsync(new AuthenticationResult { Token = token, Role = role });
            var controller = new UserController(mockLogger.Object, mockUserService.Object, mockAuthService.Object, mockFollowService.Object);

            // Act
            var result = await controller.Login(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<Dictionary<string, string>>(okResult.Value);
            Assert.Equal(token, value["Token"]);
            Assert.Equal(role, value["Role"]);
        }

    }
}
