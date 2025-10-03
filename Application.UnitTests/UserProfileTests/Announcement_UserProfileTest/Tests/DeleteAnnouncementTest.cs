using Domain.CustomExceptions;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.UserProfileTests.Announcement_UserProfileTest.Tests
{
    public class DeleteAnnouncementTest : Announcement_UserProfileTestBase
    {
        #region DeleteAnnouncement_Success_WhenAnnouncementExists
        [Fact]
        public async Task DeleteAnnouncement_Success_WhenAnnouncementExists()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var announcement = new Announcement { Id = announcementId, Title = "Test", Content = "Test content" };

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, null, null, null, default))
                .ReturnsAsync(announcement);

            _announcementRepositoryMock
                .Setup(r => r.DeleteAsync(announcement, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var ex = await Record.ExceptionAsync(() => _userProfileServiceMock.DeleteAnnouncement(userId, announcementId));

            // Assert
            Assert.Null(ex);
            _announcementRepositoryMock.Verify(r => r.DeleteAsync(announcement, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region DeleteAnnouncement_Throws_WhenAnnouncementNotFound
        [Fact]
        public async Task DeleteAnnouncement_Throws_WhenAnnouncementNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, null, null, null, default))
                .ReturnsAsync((Announcement?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _userProfileServiceMock.DeleteAnnouncement(userId, announcementId));
        }
        #endregion
    }
}
