using Application.DTOs.Announcement.Response;
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
    public class GetAllAnnouncementsTest : AnnouncementServiceTestBase
    {
        #region GetAllAnnouncements_ShouldReturnAllAnnouncements
        [Fact]
        public async Task GetAllAnnouncements_ShouldReturnAllAnnouncements()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement { Id = Guid.NewGuid(), Title = "Announcement 1", Content = "Content 1" },
                new Announcement { Id = Guid.NewGuid(), Title = "Announcement 2", Content = "Content 2" }
            };
            _announcementRepositoryMock.Setup(repo => repo.GetAllAsync(null, true, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcements);
            // Act
            var result = await _announcementService.GetAllAnnouncements();
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Announcement 1", result[0].Title);
            Assert.Equal("Announcement 2", result[1].Title);
        }
        #endregion

        #region GetAllAnnouncements_ShouldReturnEmptyList_WhenNoAnnouncementsExist
        [Fact]
        public async Task GetAllAnnouncements_ShouldReturnEmptyList_WhenNoAnnouncementsExist()
        {
            // Arrange
            var announcements = new List<Announcement>();
            _announcementRepositoryMock.Setup(repo => repo.GetAllAsync(null, true, false, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcements);
            // Act
            var result = await _announcementService.GetAllAnnouncements();
            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
        #endregion
    }
}
