using Application.DTOs.Auth.Request;
using Domain.CustomExceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.AuhtServiceTests.Tests
{
    public class ResetPasswordTest : AuthServiceTestBase
    {
        #region Success
        [Fact]
        public async Task ResetPassword_Success_RevokesRefreshTokens()
        {
            var request = new ResetPasswordRequest
            {
                Email = "user@test.com",
                NewPassword = "NewPass123!"
            };
            string token = "valid-token";
            var user = new ApplicationUser { Id = Guid.NewGuid(), Email = request.Email };

            _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x =>
                x.ResetPasswordAsync(user, token, request.NewPassword))
                .ReturnsAsync(IdentityResult.Success);

            _refreshTokenServiceMock.Setup(x =>
                x.InvalidateUserTokensAsync(user.Id))
                .Returns(Task.CompletedTask);

            await _authService.ResetPassword(token, request);

            _refreshTokenServiceMock.Verify(x =>
                x.InvalidateUserTokensAsync(user.Id),
                Times.Once);
        }
        #endregion

        #region UserNotFound
        [Fact]
        public async Task ResetPassword_UserNotFound_ThrowsException()
        {
            var request = new ResetPasswordRequest
            {
                Email = "notfound@test.com",
                NewPassword = "NewPass123!"
            };
            string token = "valid-token";
            _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _authService.ResetPassword(token, request));
        }
        #endregion

        #region ResetFails
        [Fact]
        public async Task ResetPassword_ResetFails_ThrowsValidationException()
        {
            var request = new ResetPasswordRequest
            {
                Email = "user@test.com",
                NewPassword = "NewPass123!"
            };
            string token = "bad-token";
            var user = new ApplicationUser { Email = request.Email };

            _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x =>
                x.ResetPasswordAsync(user, token, request.NewPassword))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Invalid token" }));

            var ex = await Assert.ThrowsAsync<ValidationException>(() =>
                _authService.ResetPassword(token, request));

            Assert.Contains("Invalid token", ex.Message);
        }
        #endregion
    }
}
