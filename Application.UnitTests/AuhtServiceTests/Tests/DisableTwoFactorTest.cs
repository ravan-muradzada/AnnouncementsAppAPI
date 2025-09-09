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
    public class DisableTwoFactorAuthTest : AuthServiceTestBase
    {
        #region Success
        [Fact]
        public async Task DisableTwoFactorAuth_Success_DoesNotThrowException()
        {
            Guid userId = Guid.NewGuid();
            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser { });
            _userManagerMock.Setup(um => um.SetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()));

            var exception = await Record.ExceptionAsync(() => _authService.EnableTwoFactorAuth(userId));

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.SetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()), Times.Once);
        }
        #endregion

        #region UserNotFound
        [Fact]
        public async Task DisableTwoFactorAuth_UserNotFound_ThrowsObjectNotFoundException()
        {
            Guid userId = Guid.NewGuid();
            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()));

            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _authService.EnableTwoFactorAuth(userId));

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.SetTwoFactorEnabledAsync(It.IsAny<ApplicationUser>(), It.IsAny<bool>()), Times.Never);
        }
        #endregion
    }
}
