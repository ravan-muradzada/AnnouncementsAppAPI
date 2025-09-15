using Application.DTOs.Auth.Request;
using Application.DTOs.Auth.Response;
using Domain.CustomExceptions;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.AuhtServiceTests.Tests
{
    public class GenerateNewRefreshTokenTest : AuthServiceTestBase
    {
        #region Success
        [Fact]
        public async Task RefreshToken_Success_ReturnsResponse()
        {
            RefreshTokenRequest request = new RefreshTokenRequest
            {
                RefreshToken = "sample-refresh-token"
            };

            _refreshTokenServiceMock.Setup(rsm => rsm.FindUserOfRefreshTokenAndDeleteToken(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser());
            _accessTokenServiceMock.Setup(ats => ats.GenerateToken(It.IsAny<ApplicationUser>()))
                .Returns("sample-access-token");
            _refreshTokenServiceMock.Setup(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()))
                .ReturnsAsync("sample-refresh-token");
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()));

            AuthenticatedResponse response = await _authService.GenerateNewRefreshToken(request);

            Assert.Equal("sample-access-token", response.AccessToken);
            Assert.Equal("sample-refresh-token", response.RefreshToken);
        }
        #endregion

        #region InvalidOrExpiredToken
        [Fact]
        public async Task RefreshToken_InvalidOrExpiredToken_ThrowsInvalidCredentialsException()
        {
            RefreshTokenRequest request = new RefreshTokenRequest
            {
                RefreshToken = "sample-refresh-token"
            };
            _refreshTokenServiceMock.Setup(rsm => rsm.FindUserOfRefreshTokenAndDeleteToken(It.IsAny<string>()));

            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.GenerateNewRefreshToken(request));
        }
        #endregion
    }
}
