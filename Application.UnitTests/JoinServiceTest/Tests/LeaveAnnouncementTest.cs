using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.CustomExceptions;
using Domain.Entities;
using Moq;
using Xunit;

namespace Application.UnitTests.JoinServiceTest.Tests
{
    public class LeaveAnnouncementTest : JoinServiceTestBase
    {
        #region LeaveAnnouncementAsync_RemovesUserFromAnnouncement_WhenUserIsJoined
        [Fact]
        public async Task LeaveAnnouncementAsync_RemovesUserFromAnnouncement_WhenUserIsJoined()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var announcementUser = new AnnouncementUser { AnnouncementId = announcementId, ApplicationUserId = userId };

            _announcementRepositoryMock.Setup(r => r.ExistsAsync(announcementId, null, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new ApplicationUser { Id = userId});
            _joinRepositoryMock
                .Setup(r => r.GetAnnouncementUserAsync(announcementId, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcementUser);

            _joinRepositoryMock
                .Setup(r => r.LeaveAnnouncementAsync(announcementUser, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _joinService.LeaveAnnouncementAsync(announcementId, userId);

            // Assert
            _joinRepositoryMock.Verify(r => r.LeaveAnnouncementAsync(announcementUser, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region LeaveAnnouncementAsync_ThrowsError_WhenUserIsNotJoined
        [Fact]
        public async Task LeaveAnnouncementAsync_ThrowsError_WhenUserIsNotJoined()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            _announcementRepositoryMock.Setup(r => r.ExistsAsync(announcementId, null, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new ApplicationUser { Id = userId });
            _joinRepositoryMock
                .Setup(r => r.GetAnnouncementUserAsync(announcementId, userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((AnnouncementUser?)null);

            // Act
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _joinService.LeaveAnnouncementAsync(announcementId, userId));

            // Assert
            _joinRepositoryMock.Verify(r => r.LeaveAnnouncementAsync(It.IsAny<AnnouncementUser>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion

        #region LeaveAnnouncementAsync_ThrowsError_WhenAnnouncementDoesNotExist
        [Fact]
        public async Task LeaveAnnouncementAsync_ThrowsError_WhenAnnouncementDoesNotExist()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock.Setup(r => r.ExistsAsync(announcementId, null, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new ApplicationUser { Id = userId });
            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _joinService.LeaveAnnouncementAsync(announcementId, userId));
            _joinRepositoryMock.Verify(r => r.LeaveAnnouncementAsync(It.IsAny<AnnouncementUser>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion

        #region LeaveAnnouncementAsync_ThrowsError_WhenUserDoesNotExist
        [Fact]
        public async Task LeaveAnnouncementAsync_ThrowsError_WhenUserDoesNotExist()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock.Setup(r => r.ExistsAsync(announcementId, null, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            _userManagerMock.Setup(um => um.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((ApplicationUser?)null);
            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() => _joinService.LeaveAnnouncementAsync(announcementId, userId));
            _joinRepositoryMock.Verify(r => r.LeaveAnnouncementAsync(It.IsAny<AnnouncementUser>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        #endregion
    }
}
