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
    public class EnableTwoFactorAuth : TwoFactorTestBase
    {
        #region EnableTwoFactorAuth_ShouldEnable2FA
        [Fact]
        public async Task EnableTwoFactorAuth_ShouldEnable2FA()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            { Id = userId, UserName = "testuser", Email = "mail@gmail.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString())).ReturnsAsync(user);
            _userManagerMock.Setup(um => um.SetTwoFactorEnabledAsync(user, true)).ReturnsAsync(IdentityResult.Success);

            var exception = await Record.ExceptionAsync(() => _twoFactorAuthService.EnableTwoFactorAuth(userId));
            Assert.Null(exception);
        }
        #endregion

        #region EnableTwoFactorAuth_UserNotFound
        [Fact]
        public async Task EnableTwoFactorAuth_UserNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            { Id = userId, UserName = "testuser", Email = "mail@gmail.com" };
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString()));
            
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _twoFactorAuthService.EnableTwoFactorAuth(userId));
        }
        #endregion
    }
}
