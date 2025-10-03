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

namespace Application.UnitTests.UserProfileTests.TwoFactorTest.Tests
{
    public class DisableTwoFactorAuth : TwoFactorTestBase
    {
        #region DisableTwoFactorAuth_ShouldEnable2FA
        [Fact]
        public async Task DisableTwoFactorAuth_ShouldEnable2FA()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            { Id = userId, UserName = "testuser", Email = "mail@gmail.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.SetTwoFactorEnabledAsync(user, false)).ReturnsAsync(IdentityResult.Success);

            var exception = await Record.ExceptionAsync(() => _twoFactorAuthService.DisableTwoFactorAuth(userId));
            Assert.Null(exception);
        }
        #endregion

        #region DisableTwoFactorAuth_UserNotFound
        [Fact]
        public async Task DisableTwoFactorAuth_UserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            { Id = userId, UserName = "testuser", Email = "mail@gmail.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString()));

            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _twoFactorAuthService.DisableTwoFactorAuth(userId));
        }
        #endregion
    }
}
