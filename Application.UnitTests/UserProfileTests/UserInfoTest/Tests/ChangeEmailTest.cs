using Application.DTOs.UserProfile.Request;
using Domain.CustomExceptions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.UserProfileTests.UserInfoTest.Tests
{
    public class ChangeEmailTest : UserInfoTestBase
    {
        #region ChangeEmail_WhenUserExists_SavesOtpAndSendsEmail
        [Fact]
        public async Task ChangeEmail_WhenUserExists_SavesOtpAndSendsEmail()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new ChangeEmailRequest("new@gmail.com"); 
            var user = new ApplicationUser { Id = userId, UserName = "testuser" };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            _userManagerMock.Setup(u => u.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            _redisRepositoryMock.Setup(r => r.SetStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()));

            _emailServiceMock.Setup(e => e.SendEmail(request.NewEmail, "OTP Sent!", It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _userInfoService.ChangeEmail(userId, request);

            // Assert
            _userManagerMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);
            _redisRepositoryMock.Verify(r => r.SetStringAsync(It.Is<string>(s => s.StartsWith("email_change_otp_")), It.IsAny<string>(), TimeSpan.FromMinutes(5)), Times.Once);
            _redisRepositoryMock.Verify(r => r.SetStringAsync(It.Is<string>(s => s.StartsWith("email_change_candidate_")), request.NewEmail, TimeSpan.FromMinutes(5)), Times.Once);
            _userManagerMock.Verify(u => u.UpdateAsync(user), Times.Once);
            _emailServiceMock.Verify(e => e.SendEmail(request.NewEmail, "OTP Sent!", It.IsAny<string>()), Times.Once);
        }
        #endregion

        #region ChangeEmail_WhenUserDoesNotExist_ThrowsObjectNotFoundException
        [Fact]
        public async Task ChangeEmail_WhenUserDoesNotExist_ThrowsObjectNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new ChangeEmailRequest("new@gmail.com");

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _userInfoService.ChangeEmail(userId, request));

            _userManagerMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);
            _redisRepositoryMock.Verify(r => r.SetStringAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<TimeSpan>()), Times.Never);
            _emailServiceMock.Verify(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        #endregion
    }
}
