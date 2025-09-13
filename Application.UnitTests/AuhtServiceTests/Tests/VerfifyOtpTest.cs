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
    public class VerifyOtpTest : AuthServiceTestBase
    {
        #region Success
        [Fact]
        public async Task VerifyOtp_Success_ReturnsResponse()
        {
            VerifyOtpRequest request = new VerifyOtpRequest
            {
                Email = "ravan@gmail.com",
                Otp = "12345"
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser {  });
            _accessTokenServiceMock.Setup(ats => ats.GenerateToken(It.IsAny<ApplicationUser>()))
                .Returns("fake-access-token");
            _refreshTokenServiceMock.Setup(rts => rts.GenerateRefreshToken(It.IsAny<Guid>()))
                .ReturnsAsync(new RefreshToken { Token = "our_refresh_token", ExpirationTime = DateTime.UtcNow.AddMinutes(5) });
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()));

            AuthenticatedResponse response = await _authService.VerifyOtp(request);

            Assert.Equal("fake-access-token", response.AccessToken);
            Assert.Equal("our_refresh_token", response.RefreshToken);
        }
        #endregion

        #region UserNotFound
        [Fact]
        public async Task VerifyOtp_UserNotFound_ThrowsObjectNotFound()
        {
            VerifyOtpRequest request = new VerifyOtpRequest
            {
                Email = "ravan@gmail.com",
                Otp = "12345",
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()));
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _authService.VerifyOtp(request));
        }
        #endregion

        #region OtpMismatch
        [Fact]
        public async Task VerifyOtp_OtpMismatch_ThrowsInvalidCredentialsException()
        {
            VerifyOtpRequest request = new VerifyOtpRequest
            {
                Email = "ravan@gmail.com",
                Otp = "12345",
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser {  });
            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.VerifyOtp(request));
        }
        #endregion

        #region ExpiredOtp
        [Fact]
        public async Task VerifyOtp_ExpiretOtp_ThrowsInvalidCredentials()
        {
            VerifyOtpRequest request = new VerifyOtpRequest
            {
                Email = "ravan@gmail.com",
                Otp = "12345",
            };

            _userManagerMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser {  });
            await Assert.ThrowsAsync<InvalidCredentialsException>(() => _authService.VerifyOtp(request));
        }
        #endregion
    }
}
