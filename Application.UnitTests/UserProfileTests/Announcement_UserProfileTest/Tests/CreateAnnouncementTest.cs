using Application.DTOs.Announcement.Request;
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
    public class CreateAnnouncementTest : Announcement_UserProfileTestBase
    {
        #region CreateAnnouncement_ReturnsMappedResponse_WhenSuccessful
        [Fact]
        public async Task CreateAnnouncement_ReturnsMappedResponse_WhenSuccessful()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateAnnouncementRequest("Test Title", "Test Content", "General", null);

            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Category = request.Category
            };

            _announcementRepositoryMock
                .Setup(r => r.AddAync(It.IsAny<Announcement>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcement);

            // Act
            var result = await _userProfileServiceMock.CreateAnnouncement(userId, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(announcement.Id, result.Id);
            Assert.Equal(request.Title, result.Title);
            Assert.Equal(request.Content, result.Content);
            Assert.Equal(request.Category, result.Category);

            _announcementRepositoryMock.Verify(r => r.AddAync(
                It.Is<Announcement>(a => a.Title == request.Title && a.Content == request.Content),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
        #endregion

        #region CreateAnnouncement_ThrowsException_WhenRepositoryFails
        [Fact]
        public async Task CreateAnnouncement_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var request = new CreateAnnouncementRequest("Test Title", "Test Content", "General", null);


            _announcementRepositoryMock
                .Setup(r => r.AddAync(It.IsAny<Announcement>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _userProfileServiceMock.CreateAnnouncement(userId, request));

            Assert.Equal("DB error", ex.Message);
        }
        #endregion
    }
}
