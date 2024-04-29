using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using MusicStreamingService_BackEnd.Database;
using MusicStreamingService_BackEnd.Entities;
using MusicStreamingService_BackEnd.Models;
using MusicStreamingService_BackEnd.Services.PlayHistoryService;
using NUnit.Framework;

namespace MusicStreamingService_BackEnd.Tests
{
    [TestFixture]
    public class PlayHistoryServiceTests
    {
        [Test]
        public async Task GetPlayHistoryByUserId_ReturnsCorrectPlayHistory()
        {
            // Arrange
            var userId = 1;
            var token = "sampleToken";
            var expectedPlayHistory = new List
            <PlayHistory>
            {
                new PlayHistory { UserId = userId, SongId = 1, DatePlayed = DateTime.Now },
                new PlayHistory { UserId = userId, SongId = 2, DatePlayed = DateTime.Now }
            };

            var mockDbContext = new Mock<AppDbContext>();
            mockDbContext.Setup(db => db.PlayHistories)
                         .ReturnsDbSet(expectedPlayHistory);

            var mockTokenService = new Mock<TokenService>();
            mockTokenService.Setup(ts => ts.VerifyToken(token))
                            .Returns(new ClaimsPrincipal(new ClaimsIdentity(new[]
                            {
                                new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                            })));

            var service = new PlayHistoryService(mockDbContext.Object);

            // Act
            var result = await service.GetPlayHistoryByUserId(token);

            // Assert
            Assert.AreEqual(expectedPlayHistory.Count, result.Count);
            Assert.IsTrue(result.All(ph => ph.UserId == userId));
        }

        [Test]
        public async Task AddPlayHistory_ValidInput_AddsPlayHistoryToDbContext()
        {
            // Arrange
            var userId = 1;
            var songId = 1;
            var datePlayed = DateTime.Now;
            var mockDbContext = new Mock<AppDbContext>();
            var service = new PlayHistoryService(mockDbContext.Object);

            // Act
            await service.AddPlayHistory(userId, songId, datePlayed);

            // Assert
            mockDbContext.Verify(db => db.PlayHistories.Add(It.IsAny
            <PlayHistory>()), Times.Once);
            mockDbContext.Verify(db => db.SaveChangesAsync(), Times.Once);
        }
    }
}