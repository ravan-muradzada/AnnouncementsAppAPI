using Application.DTOs.UserProfile.Request;
using Application.DTOs.UserProfile.Response;
using Domain.CustomExceptions;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.UserProfileTests.UserInfoTest.Tests
{
    public class ChangeUsernameTest : UserInfoTestBase
    {
        #region Success
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public async Task ChangeUsername_Success_ReturnsResponse(int CURRENT_COUNT_OF_CHANGE)
        {
            Guid userId = Guid.NewGuid();
            ChangeUsernameRequest request = new ChangeUsernameRequest("new-username");

            ApplicationUser user = new ApplicationUser
            {
                UserName = "previous-username",
                LimitDate = DateTime.UtcNow.AddDays(1),
                currentCountOfChange = CURRENT_COUNT_OF_CHANGE
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()));
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()));

            var response = await _userInfoService.ChangeUsername(userId, request);

            Assert.IsType<UserProfileResponse>(response);
            Assert.Equal(request.NewUsername, response.UserName);
            Assert.Equal(user.UserName, response.UserName);

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }
        #endregion

        #region UserNotFound
        [Fact]
        public async Task ChangeUsername_UserNotFound_ObjectNotFoundException()
        {
            Guid userId = Guid.NewGuid();
            ChangeUsernameRequest request = new ChangeUsernameRequest("new-username");

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()));

            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _userInfoService.ChangeUsername(userId, request));

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
        #endregion

        #region EmptyUsername
        [Theory]
        [InlineData("")]
        public async Task ChangeUsername_NullOrEmptyUsername_ThrowsNullParameterException(string USERNAME_DATA)
        {
            Guid userId = Guid.NewGuid();
            ChangeUsernameRequest request = new ChangeUsernameRequest(USERNAME_DATA);

            ApplicationUser user = new ApplicationUser
            {
                UserName = "previous-username"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);

            await Assert.ThrowsAsync<NullParameterException>(() => _userInfoService.ChangeUsername(userId, request));

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Never);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
        #endregion

        #region UserWithThisUsernameAlreadyExists
        [Fact]
        public async Task ChangeUsername_UserWithThisUsernameAlreadyExists_ThrowsConflictException()
        {
            Guid userId = Guid.NewGuid();
            ChangeUsernameRequest request = new ChangeUsernameRequest("new-username");

            ApplicationUser user = new ApplicationUser
            {
                UserName = "previous-username"
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync(new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = request.NewUsername
                });

            await Assert.ThrowsAsync<ConflictException>(() => _userInfoService.ChangeUsername(userId, request));

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
        #endregion

        #region ThreeTimesChangedAlready
        [Theory]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        // ....
        public async Task ChangeUsername_ThreeTimesChangedAlready_ThrowsNotAllowedException(int CURRENT_COUNT_OF_CHANGE)
        {
            Guid userId = Guid.NewGuid();
            ChangeUsernameRequest request = new ChangeUsernameRequest("new-username");

            ApplicationUser user = new ApplicationUser
            {
                UserName = "previous-username",
                LimitDate = DateTime.UtcNow.AddDays(1),
                currentCountOfChange = CURRENT_COUNT_OF_CHANGE
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()));

            await Assert.ThrowsAsync<NotAllowedException>(() => _userInfoService.ChangeUsername(userId, request));

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Never);
        }
        #endregion

        #region LimitDatePassed
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(3)]
        [InlineData(5)]
        public async Task ChangeUsername_LimitDatePassed_ReturnsResponse(int CURRENT_COUNT_OF_CHANGE)
        {
            Guid userId = Guid.NewGuid();
            ChangeUsernameRequest request = new ChangeUsernameRequest("new-username");

            ApplicationUser user = new ApplicationUser
            {
                UserName = "previous-username",
                LimitDate = DateTime.UtcNow.AddDays(-1),
                currentCountOfChange = CURRENT_COUNT_OF_CHANGE
            };

            _userManagerMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            _userManagerMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()));
            _userManagerMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()));

            var response = await _userInfoService.ChangeUsername(userId, request);

            Assert.IsType<UserProfileResponse>(response);
            Assert.Equal(request.NewUsername, response.UserName);
            Assert.Equal(user.UserName, response.UserName);

            _userManagerMock.Verify(um => um.FindByIdAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.FindByNameAsync(It.IsAny<string>()), Times.Once);
            _userManagerMock.Verify(um => um.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);
        }
        #endregion
    }
}
