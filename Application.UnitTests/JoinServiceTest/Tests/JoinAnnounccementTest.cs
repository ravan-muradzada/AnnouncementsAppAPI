using Domain.CustomExceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.JoinServiceTest.Tests
{
    public class JoinAnnounccementTest : JoinServiceTestBase
    {
        #region JoinAnnouncementAsync_ValidInput_ShouldJoinSuccessfully
        [Fact]
        public async Task JoinAnnouncementAsync_ValidInput_ShouldJoinSuccessfully()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock.Setup(repo => repo.ExistsAsync(announcementId, null, false, default))
                .ReturnsAsync(true);
            _userManagerMock.Setup(manager => manager.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new Domain.Entities.ApplicationUser { Id = userId });
            _joinRepositoryMock.Setup(repo => repo.CheckJoin(announcementId, userId, default))
                .ReturnsAsync(false);
            // Act
            await _joinService.JoinAnnouncementAsync(announcementId, userId);
            // Assert
            _joinRepositoryMock.Verify(repo => repo.JoinAnnouncementAsync(It.Is<Domain.Entities.AnnouncementUser>(au => au.AnnouncementId == announcementId && au.ApplicationUserId == userId), default), Times.Once);
        }
        #endregion

        #region JoinAnnouncementAsync_AnnouncementDoesNotExist_ShouldThrowException
        [Fact]
        public async Task JoinAnnouncementAsync_AnnouncementDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock.Setup(repo => repo.ExistsAsync(announcementId, null, false, default))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _joinService.JoinAnnouncementAsync(announcementId, userId));
        }
        #endregion

        #region JoinAnnouncementAsync_UserDoesNotExist_ShouldThrowException
        [Fact]
        public async Task JoinAnnouncementAsync_UserDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock.Setup(repo => repo.ExistsAsync(announcementId, null, false, default))
                .ReturnsAsync(true);
            _userManagerMock.Setup(manager => manager.FindByIdAsync(userId.ToString()))
                .ReturnsAsync((Domain.Entities.ApplicationUser?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _joinService.JoinAnnouncementAsync(announcementId, userId));
        }
        #endregion

        #region JoinAnnouncementAsync_UserAlreadyJoined_ShouldNotJoinAgain
        [Fact]
        public async Task JoinAnnouncementAsync_UserAlreadyJoined_ShouldNotJoinAgain()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            _announcementRepositoryMock.Setup(repo => repo.ExistsAsync(announcementId, null, false, default))
                .ReturnsAsync(true);
            _userManagerMock.Setup(manager => manager.FindByIdAsync(userId.ToString()))
                .ReturnsAsync(new Domain.Entities.ApplicationUser { Id = userId });
            _joinRepositoryMock.Setup(repo => repo.CheckJoin(announcementId, userId, default))
                .ReturnsAsync(true);

            // Assert
            await Assert.ThrowsAsync<ConflictException>(() =>
                _joinService.JoinAnnouncementAsync(announcementId, userId));
        }
        #endregion
    }
}
