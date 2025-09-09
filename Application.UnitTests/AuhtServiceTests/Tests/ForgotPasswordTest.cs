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
    public class ForgotPasswordTest : AuthServiceTestBase
    {
        #region Success
        [Fact]
        public async Task ForgotPassword_Success_SendMail()
        {
            var request = new ForgotPasswordRequest { Email = "user@test.com" };
            var user = new ApplicationUser { Email = request.Email };

            _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userManagerMock.Setup(x => x.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("dummy-token");

            _emailServiceMock.Setup(x =>
                x.SendEmail(request.Email, "Reset Password Link", It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _authService.ForgotPassword(request);

            // Assert
            _emailServiceMock.Verify(x =>
                x.SendEmail(request.Email, "Reset Password Link",
                    It.Is<string>(msg => msg.Contains("dummy-token"))),
                Times.Once);
        }
        #endregion

        #region UserNotFound
        [Fact]
        public async Task ForgotPassword_UserNotFound_ThrowsException()
        {
            // Arrange
            var request = new ForgotPasswordRequest { Email = "notfound@test.com" };
            _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email))
                .ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _authService.ForgotPassword(request));
        }
        #endregion
    }
}
