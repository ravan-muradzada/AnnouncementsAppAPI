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
    public class LoginTest : AuthServiceTestBase
    {
        #region Success
        [Fact]
        public async Task Login_Success_ReturnsResponse()
        {
            LoginRequest request = new LoginRequest
            {
                Username = "ravan@gmail.com",
                Password = "Ravan's_password1234"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { EmailConfirmed = true });
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(true);
            _userManagerMock.Setup(um => um.GetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(false);
            _accessTokenServiceMock.Setup(atc => atc.GenerateToken(It.IsAny<ApplicationUser>()))
                .Returns("sample-access-token");
            _refreshTokenServiceMock.Setup(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()))
                .ReturnsAsync("sample-refresh-token");
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()));

            AuthenticatedResponse? response = await _authService.Login(request);
            Assert.Equal("sample-access-token", response?.AccessToken);
            Assert.Equal("sample-refresh-token", response?.RefreshToken);

            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            _accessTokenServiceMock.Verify(atc => atc.GenerateToken(It.IsAny<ApplicationUser>()), Times.Once);
            _refreshTokenServiceMock.Verify(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _userManagerMock.Verify(um => um.GetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _twoFactorServiceMock.Verify(tfs => tfs.SendTwoFactorCode(It.IsAny<ApplicationUser>()), Times.Never);
        }
        #endregion

        #region WrongEmail
        [Fact]
        public async Task Login_WrongEmail_ThrowsInvalidCredentialsException()
        {
            LoginRequest request = new LoginRequest
            {
                Username = "ravan@gmail.com",
                Password = "123456"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()));
            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.Login(request));
        }
        #endregion

        #region WrongPassword
        [Fact]
        public async Task Login_WrongPassword_ThrowsInvalidCredentialsException()
        {
            LoginRequest request = new LoginRequest
            {
                Username = "ravan@gmail.com",
                Password = "Ravan_1234"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { });
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.Login(request));
        }
        #endregion

        #region EmailNotConfirmed
        [Fact]
        public async Task Login_EmailNotConfirmed_ThrowsNotConfirmedException()
        {
            LoginRequest request = new LoginRequest
            {
                Username = "ravan@gmail.com",
                Password = "Ravan's_password1234"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { EmailConfirmed = false });
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<NotConfirmedException>(() => _authService.Login(request));
        }
        #endregion

        #region TwoFactorAuthEnabled
        [Fact]
        public async Task Login_TwoFactorAuthEnabled_ReturnsNull()
        {
            LoginRequest request = new LoginRequest
            {
                Username = "ravan@gmail.com",
                Password = "Ravan's_password1234"
            };

            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { EmailConfirmed = true });
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), request.Password))
                .ReturnsAsync(true);
            _userManagerMock.Setup(um => um.GetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(true);
            _twoFactorServiceMock.Setup(tfs => tfs.SendTwoFactorCode(It.IsAny<ApplicationUser>()));

            var response = await _authService.Login(request);

            Assert.Null(response);

            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), request.Password), Times.Once);
            _accessTokenServiceMock.Verify(atc => atc.GenerateToken(It.IsAny<ApplicationUser>()), Times.Never);
            _refreshTokenServiceMock.Verify(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()), Times.Never);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
            _userManagerMock.Verify(um => um.GetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>()), Times.Once);
            _twoFactorServiceMock.Verify(tfs => tfs.SendTwoFactorCode(It.IsAny<ApplicationUser>()), Times.Once);
        }
        #endregion
    }
}
