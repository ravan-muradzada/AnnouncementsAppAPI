using Application.DTOs.Auth.Request;
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
    public class VerifyTwoFactorTest : AuthServiceTestBase
    {
        #region Success
        [Fact]
        public async Task VerifyTwoFactor_Success_ReturnsResponse()
        {
            VerifyTwoFactorAuthRequest request = new VerifyTwoFactorAuthRequest
            {
                Email = "example@gmail.com",
                Code = "dfsdfsgjk1235445"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { });
            _twoFactorServiceMock.Setup(tfs => tfs.VerifyTwoFactorCode(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(true);
            _accessTokenServiceMock.Setup(atc => atc.GenerateToken(It.IsAny<ApplicationUser>()))
                .Returns("sample-access-token");
            _refreshTokenServiceMock.Setup(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()))
                .ReturnsAsync(new RefreshToken { Token = "sample-refresh-token", ExpirationTime = DateTime.UtcNow.AddMinutes(3) });
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()));

            var response = await _authService.VerifyTwoFactorAuth(request);

            Assert.Equal("sample-access-token", response.AccessToken);
            Assert.Equal("sample-refresh-token", response.RefreshToken);

            _userManagerMock.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _accessTokenServiceMock.Verify(atc => atc.GenerateToken(It.IsAny<ApplicationUser>()), Times.Once);
            _refreshTokenServiceMock.Verify(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _twoFactorServiceMock.Verify(tfs => tfs.VerifyTwoFactorCode(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }
        #endregion

        #region UserNotFound
        [Fact]
        public async Task VerifyTwoFactor_UserNotFound_ThrowsObjectNotFoundException()
        {
            VerifyTwoFactorAuthRequest request = new VerifyTwoFactorAuthRequest
            {
                Email = "example@gmail.com",
                Code = "dfsdfsgjk1235445"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()));

            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _authService.VerifyTwoFactorAuth(request));

            _userManagerMock.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _accessTokenServiceMock.Verify(atc => atc.GenerateToken(It.IsAny<ApplicationUser>()), Times.Never);
            _refreshTokenServiceMock.Verify(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()), Times.Never);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _twoFactorServiceMock.Verify(tfs => tfs.VerifyTwoFactorCode(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }
        #endregion

        #region TwoFactorAuthFailed
        [Fact]
        public async Task VerifyTwoFactor_TwoFactorAuthFailed_ThrowsTwoFactorAuthFailedException()
        {
            VerifyTwoFactorAuthRequest request = new VerifyTwoFactorAuthRequest
            {
                Email = "example@gmail.com",
                Code = "dfsdfsgjk1235445"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { });
            _twoFactorServiceMock.Setup(tfs => tfs.VerifyTwoFactorCode(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<TwoFactorAuthFailedException>(() => _authService.VerifyTwoFactorAuth(request));

            _userManagerMock.Verify(um => um.FindByEmailAsync(It.IsAny<string>()), Times.Once);
            _accessTokenServiceMock.Verify(atc => atc.GenerateToken(It.IsAny<ApplicationUser>()), Times.Never);
            _refreshTokenServiceMock.Verify(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()), Times.Never);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _twoFactorServiceMock.Verify(tfs => tfs.VerifyTwoFactorCode(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }
        #endregion
    }
}
