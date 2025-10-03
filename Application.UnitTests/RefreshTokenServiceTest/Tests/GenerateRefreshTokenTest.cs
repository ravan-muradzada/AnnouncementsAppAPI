using Application.InternalServices;
using Domain.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.RefreshTokenServiceTest.Tests
{
    public class GenerateRefreshTokenTest : RefreshTokenServiceTestBase
    {
        #region GenerateRefreshToken_ReturnsToken_AndStoresInRedis
        [Fact]
        public async Task GenerateRefreshToken_ReturnsToken_AndStoresInRedis()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _redisRepositoryMock
                .Setup(r => r.SetStringAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<TimeSpan>()));

            // Act
            var token = await _refreshTokenService.GenerateRefreshToken(userId);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token)); // token is generated
            Assert.DoesNotContain(" ", token); // base64 string check (rough)

            _redisRepositoryMock.Verify(r => r.SetStringAsync(
                It.IsAny<string>(),
                "valid",
                It.IsAny<TimeSpan>()),
                Times.Once);
        }
        #endregion

        #region GenerateRefreshToken_UsesDefaultExpiration_WhenConfigMissing
        [Fact]
        public async Task GenerateRefreshToken_UsesDefaultExpiration_WhenConfigMissing()
        {
            // Arrange
            var userId = Guid.NewGuid();
            
            // Act
            var token = await _refreshTokenService.GenerateRefreshToken(userId);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
            _redisRepositoryMock.Verify(r => r.SetStringAsync(
                It.Is<string>(k => k.StartsWith($"Refresh_Token:{userId}:")),
                "valid",
                TimeSpan.FromMinutes(10)), // default from code
                Times.Once);
        }
        #endregion
    }
}
