using Domain.CustomExceptions;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.RefreshTokenServiceTest.Tests
{
    public class FindUserOfRefreshTokenAndDeleteTokenTest : RefreshTokenServiceTestBase
    {
        #region FindUserOfRefreshTokenAndDeleteToken_ReturnsUser_WhenTokenValid
        [Fact]
        public async Task FindUserOfRefreshTokenAndDeleteToken_ReturnsUser_WhenTokenValid()
        {
            // Arrange
            var refreshToken = "sampleToken";
            var userId = Guid.NewGuid();
            var redisKey = $"Refresh_Token:{userId}:{refreshToken}";

            var user = new ApplicationUser { Id = userId, UserName = "testuser" };

            _redisRepositoryMock
                .Setup(r => r.GetByPattern($"Refresh_Token:*:{refreshToken}"))
                .Returns(new List<string> { redisKey });

            _userManagerMock
                .Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _redisRepositoryMock
                .Setup(r => r.DeleteAsync(redisKey));

            // Act
            var result = await _refreshTokenService.FindUserOfRefreshTokenAndDeleteToken(refreshToken);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId.ToString(), result.Id.ToString());
            _redisRepositoryMock.Verify(r => r.DeleteAsync(redisKey), Times.Once);
        }
        #endregion

        #region FindUserOfRefreshTokenAndDeleteToken_Throws_WhenTokenNotFound
        [Fact]
        public async Task FindUserOfRefreshTokenAndDeleteToken_Throws_WhenTokenNotFound()
        {
            // Arrange
            var refreshToken = "missingToken";

            _redisRepositoryMock
                .Setup(r => r.GetByPattern($"Refresh_Token:*:{refreshToken}"))
                .Returns(new List<string>());

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _refreshTokenService.FindUserOfRefreshTokenAndDeleteToken(refreshToken));
        }
        #endregion

        #region FindUserOfRefreshTokenAndDeleteToken_Throws_WhenKeyInvalid
        [Fact]
        public async Task FindUserOfRefreshTokenAndDeleteToken_Throws_WhenKeyInvalid()
        {
            // Arrange
            var refreshToken = "invalidToken";
            var invalidKey = $"Refresh_Token:{refreshToken}"; // only 2 parts

            _redisRepositoryMock
                .Setup(r => r.GetByPattern($"Refresh_Token:*:{refreshToken}"))
                .Returns(new List<string> { invalidKey });

            // Act & Assert
            await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
                _refreshTokenService.FindUserOfRefreshTokenAndDeleteToken(refreshToken));
        }
        #endregion

        #region FindUserOfRefreshTokenAndDeleteToken_Throws_WhenUserNotFound
        [Fact]
        public async Task FindUserOfRefreshTokenAndDeleteToken_Throws_WhenUserNotFound()
        {
            // Arrange
            var refreshToken = "sampleToken";
            var userId = Guid.NewGuid();
            var redisKey = $"Refresh_Token:{userId}:{refreshToken}";

            _redisRepositoryMock
                .Setup(r => r.GetByPattern($"Refresh_Token:*:{refreshToken}"))
                .Returns(new List<string> { redisKey });

            _userManagerMock
                .Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _refreshTokenService.FindUserOfRefreshTokenAndDeleteToken(refreshToken));
        }
        #endregion
    }
}
