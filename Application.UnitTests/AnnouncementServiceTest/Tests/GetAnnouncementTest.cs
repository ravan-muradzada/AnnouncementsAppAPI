using Domain.CustomExceptions;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.AnnouncementServiceTest.Tests
{
    public class GetAnnouncementTest : AnnouncementServiceTestBase
    {
        #region GetAnnouncement_ExistingId_ReturnsAnnouncementResponse
        [Fact]
        public async Task GetAnnouncement_ExistingId_ReturnsAnnouncementResponse()
        {
            var announcements = new List<Announcement>
            {
                new Announcement { Id = Guid.NewGuid(), Title = "Announcement 1", Content = "Content 1" },
                new Announcement { Id = Guid.NewGuid(), Title = "Announcement 2", Content = "Content 2" }
            };
            // Arrange
            var existingAnnouncement = announcements.First();
            var existingId = existingAnnouncement.Id;

            _announcementRepositoryMock.Setup(repo => repo.GetByIdAsync(existingId, null, true, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(existingAnnouncement);

            // Act
            var result = await _announcementService.GetAnnouncement(existingId);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(existingAnnouncement.Id, result.Id);
            Assert.Equal(existingAnnouncement.Title, result.Title);
            Assert.Equal(existingAnnouncement.Content, result.Content);
        }
        #endregion

        #region GetAnnouncement_NonExistingId_ThrowsObjectNotFoundException
        [Fact]
        public async Task GetAnnouncement_NonExistingId_ThrowsObjectNotFoundException()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();
            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(async () =>
            {
                await _announcementService.GetAnnouncement(nonExistingId);
            });
        }
        #endregion
    }
}
