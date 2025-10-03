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
    public class GetUserTest : UserInfoTestBase
    {
        #region GetUser_WhenUserExists_ReturnsUserProfileResponse
        [Fact]
        public async Task GetUser_WhenUserExists_ReturnsUserProfileResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = "testuser",
                Email = "test@example.com"
            };

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(user);

            // Act
            var result = await _userInfoService.GetUser(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal("testuser", result.UserName);
            Assert.Equal("test@example.com", result.Email);

            _userManagerMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);
        }
        #endregion

        #region GetUser_WhenUserDoesNotExist_ThrowsObjectNotFoundException
        [Fact]
        public async Task GetUser_WhenUserDoesNotExist_ThrowsObjectNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();

            _userManagerMock.Setup(u => u.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _userInfoService.GetUser(userId));

            _userManagerMock.Verify(u => u.FindByIdAsync(userId.ToString()), Times.Once);
        }
        #endregion
    }
}
