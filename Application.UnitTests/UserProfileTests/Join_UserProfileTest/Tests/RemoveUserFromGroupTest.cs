using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.UserProfileTests.Join_UserProfileTest.Tests
{
    public class RemoveUserFromGroupTest : Join_UserProfileTestBase
    {
        #region RemoveUserFromGroup_AnnouncementNotFound_ThrowsObjectNotFoundException
        [Fact]
        public async Task RemoveUserFromGroup_AnnouncementNotFound_ThrowsObjectNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock
                .Setup(repo => repo.ExistsAsync(announcementId, authorId, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            // Act & Assert
            await Assert.ThrowsAsync<Domain.CustomExceptions.ObjectNotFoundException>(() =>
                _join_UserProfileService.RemoveUserFromGroup(authorId, announcementId, userId));
        }
        #endregion

        #region RemoveUserFromGroup_UserNotInAnnouncement_ThrowsObjectNotFoundException
        [Fact]
        public async Task RemoveUserFromGroup_UserNotInAnnouncement_ThrowsObjectNotFoundException()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock
                .Setup(repo => repo.ExistsAsync(announcementId, authorId, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _joinRepositoryMock
                .Setup(repo => repo.GetAnnouncementUserAsync(announcementId, userId, default))
                .ReturnsAsync((Domain.Entities.AnnouncementUser?)null);
            // Act & Assert
            await Assert.ThrowsAsync<Domain.CustomExceptions.ObjectNotFoundException>(() =>
                _join_UserProfileService.RemoveUserFromGroup(authorId, announcementId, userId));
        }
        #endregion

        #region RemoveUserFromGroup_ValidInput_RemovesUserFromAnnouncement
        [Fact]
        public async Task RemoveUserFromGroup_ValidInput_RemovesUserFromAnnouncement()
        {
            // Arrange
            var authorId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var announcementUser = new Domain.Entities.AnnouncementUser
            {
                AnnouncementId = announcementId,
                ApplicationUserId = userId,
                JoinedAt = DateTime.UtcNow
            };
            _announcementRepositoryMock
                .Setup(repo => repo.ExistsAsync(announcementId, authorId, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _joinRepositoryMock
                .Setup(repo => repo.GetAnnouncementUserAsync(announcementId, userId, default))
                .ReturnsAsync(announcementUser);
            _joinRepositoryMock
                .Setup(repo => repo.LeaveAnnouncementAsync(announcementUser, default))
                .Returns(Task.CompletedTask)
                .Verifiable();
            // Act
            await _join_UserProfileService.RemoveUserFromGroup(authorId, announcementId, userId);
            // Assert
            _joinRepositoryMock.Verify(repo => repo.LeaveAnnouncementAsync(announcementUser, default), Times.Once);
        }
        #endregion
    }
}
