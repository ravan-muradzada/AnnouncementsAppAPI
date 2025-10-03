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
    public class GetAnnouncementByIdTest : Announcement_UserProfileTestBase
    {
        #region GetAnnouncementById_WhenAnnouncementExists_ReturnsAnnouncementResponse
        [Fact]
        public async Task GetAnnouncementById_WhenAnnouncementExists_ReturnsAnnouncementResponse()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var announcement = new Announcement
            {
                Id = announcementId,
                Title = "Test Title",
                Content = "Test Content"
            };

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, userId, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcement);

            // Act
            var result = await _userProfileServiceMock.GetAnnouncementById(userId, announcementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(announcement.Id, result.Id);
            Assert.Equal("Test Title", result.Title);
            Assert.Equal("Test Content", result.Content);

            _announcementRepositoryMock.Verify(r =>
                r.GetByIdAsync(announcementId, userId, null, null, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region GetAnnouncementById_WhenAnnouncementDoesNotExist_ThrowsObjectNotFoundException
        [Fact]
        public async Task GetAnnouncementById_WhenAnnouncementDoesNotExist_ThrowsObjectNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, userId, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Announcement?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _userProfileServiceMock.GetAnnouncementById(userId, announcementId));

            _announcementRepositoryMock.Verify(r =>
                r.GetByIdAsync(announcementId, userId, null, null, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion
    }
}
