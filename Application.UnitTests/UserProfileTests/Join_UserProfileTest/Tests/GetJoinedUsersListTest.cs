using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.UserProfileTests.Join_UserProfileTest.Tests
{
    public class GetJoinedUsersListTest : Join_UserProfileTestBase
    {
        #region GetJoinedUsersList_AnnouncementNotFound_ThrowsObjectNotFoundException
        [Fact]
        public async Task GetJoinedUsersList_AnnouncementNotFound_ThrowsObjectNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            _announcementRepositoryMock
                .Setup(repo => repo.ExistsAsync(announcementId, authorId, null, default))
                .ReturnsAsync(false);
            // Act & Assert
            await Assert.ThrowsAsync<Domain.CustomExceptions.ObjectNotFoundException>(() =>
                _join_UserProfileService.GetJoinedUsersList(authorId, announcementId));
        }
        #endregion

        #region GetJoinedUsersList_ValidRequest_ReturnsJoinedUsersList
        [Fact]
        public async Task GetJoinedUsersList_ValidRequest_ReturnsJoinedUsersList()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var joinedUsers = new List<Domain.Entities.AnnouncementUser>
            {
                new Domain.Entities.AnnouncementUser
                {
                    ApplicationUserId = Guid.NewGuid(),
                    User = new Domain.Entities.ApplicationUser { UserName = "User1" },
                    JoinedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Domain.Entities.AnnouncementUser
                {
                    ApplicationUserId = Guid.NewGuid(),
                    User = new Domain.Entities.ApplicationUser { UserName = "User2" },
                    JoinedAt = DateTime.UtcNow.AddDays(-1)
                }
            };
            _announcementRepositoryMock
                .Setup(repo => repo.ExistsAsync(announcementId, authorId, null, default))
                .ReturnsAsync(true);
            _joinRepositoryMock
                .Setup(repo => repo.GetAnnouncementUsers(announcementId, default))
                .ReturnsAsync(joinedUsers);
            // Act
            var result = await _join_UserProfileService.GetJoinedUsersList(authorId, announcementId);
            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("User1", result[0].Username);
            Assert.Equal("User2", result[1].Username);
        }
        #endregion
    }
}
