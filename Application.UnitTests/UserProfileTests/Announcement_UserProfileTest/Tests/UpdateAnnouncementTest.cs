using Application.DTOs.Announcement.Request;
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
    public class UpdateAnnouncementTest : Announcement_UserProfileTestBase
    {
        #region UpdateAnnouncement_Throws_WhenAnnouncementNotFound
        [Fact]
        public async Task UpdateAnnouncement_Throws_WhenAnnouncementNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, userId, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Announcement?)null);

            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _userProfileServiceMock.UpdateAnnouncement(userId, announcementId, new UpdateAnnouncementRequest("title", "content", "category", null)));
        }
        #endregion

        #region UpdateAnnouncement_UpdatesFieldsAndResetsIsPublished
        [Fact]
        public async Task UpdateAnnouncement_UpdatesFieldsAndResetsIsPublished()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var announcement = new Announcement
            {
                Id = announcementId,
                Title = "Old Title",
                Content = "Old Content",
                Category = "OldCat",
                IsPublished = true
            };

            var request = new UpdateAnnouncementRequest("New Title", "New Content", "NewCat", DateTime.UtcNow.AddDays(7));

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, userId, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcement);

            _announcementRepositoryMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<Announcement>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userProfileServiceMock.UpdateAnnouncement(userId, announcementId, request);

            // Assert
            Assert.Equal("New Title", result.Title);
            Assert.Equal("New Content", result.Content);
            Assert.Equal("NewCat", result.Category);
            Assert.False(result.IsPublished);
            _announcementRepositoryMock.Verify(r => r.SaveChangesAsync(announcement, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region UpdateAnnouncement_OnlyUpdatesNonNullFields
        [Fact]
        public async Task UpdateAnnouncement_OnlyUpdatesNonNullFields()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcementId = Guid.NewGuid();
            var announcement = new Announcement
            {
                Id = announcementId,
                Title = "Old Title",
                Content = "Old Content",
                Category = "OldCat",
                IsPublished = true
            };

            var request = new UpdateAnnouncementRequest(null, "Updated Content", null, null);

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, userId, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcement);

            _announcementRepositoryMock
                .Setup(r => r.SaveChangesAsync(It.IsAny<Announcement>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _userProfileServiceMock.UpdateAnnouncement(userId, announcementId, request);

            // Assert
            Assert.Equal("Old Title", result.Title); // unchanged
            Assert.Equal("Updated Content", result.Content); // changed
            Assert.Equal("OldCat", result.Category); // unchanged
            Assert.False(result.IsPublished); // reset
        }
        #endregion
    }
}
