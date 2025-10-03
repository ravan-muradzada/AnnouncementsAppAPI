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
    public class ChangePasswordTest : UserInfoTestBase
    {
        #region ChangePassword_Success
        [Fact]
        public async Task ChangePassword_Success()
        {
            Guid userId = Guid.NewGuid();
            ApplicationUser user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "example@gmail.com",
                PasswordHash = "OldPasswordHash"
            };

            ChangePasswordRequest request = new ChangePasswordRequest("previous", "new");
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(um => um.UpdateSecurityStampAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            _refreshTokenServiceMock.Setup(rts => rts.InvalidateUserTokensAsync(userId));

            var exception = await Record.ExceptionAsync(() => _userInfoService.ChangePassword(userId, request));
            Assert.Null(exception);
        }
        #endregion

        #region ChangePassword_UserNotFound
        [Fact]
        public async Task ChangePassword_UserNotFound()
        {
            Guid userId = Guid.NewGuid();
            ChangePasswordRequest request = new ChangePasswordRequest("previous", "new");
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync((ApplicationUser?)null);
            var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(() => _userInfoService.ChangePassword(userId, request));
            Assert.Equal("User not found!", exception.Message);
        }
        #endregion

        #region ChangePassword_InvalidCurrentPassword
        [Fact]
        public async Task ChangePassword_InvalidCurrentPassword()
        {
            Guid userId = Guid.NewGuid();
            ApplicationUser user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "mail@gmail.com",
                PasswordHash = "OldPasswordHash"
            };

            ChangePasswordRequest request = new ChangePasswordRequest("wrongprevious", "new");
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);
            var exception = await Assert.ThrowsAsync<InvalidCredentialsException>(() => _userInfoService.ChangePassword(userId, request));
        }
        #endregion

        #region ChangePassword_IdentityError
        [Fact]
        public async Task ChangePassword_IdentityError()
        {
            Guid userId = Guid.NewGuid();
            ApplicationUser user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "mail@gmail.com",
                PasswordHash = "passwordHash"
            };

            ChangePasswordRequest request = new ChangePasswordRequest("previous", "new");
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            _userManagerMock.Setup(um => um.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password change failed" }));
            var exception = await Assert.ThrowsAsync<IdentityException>(() => _userInfoService.ChangePassword(userId, request));
            Assert.Contains("Password change failed", exception.Message);
        }
        #endregion
    }
}