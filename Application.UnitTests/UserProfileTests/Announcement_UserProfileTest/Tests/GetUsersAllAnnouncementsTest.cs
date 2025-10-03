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
    public class GetUsersAllAnnouncementsTest : Announcement_UserProfileTestBase
    {
        #region GetUsersAllAnnouncements_WhenAnnouncementsExist_ReturnsMappedAnnouncements
        [Fact]
        public async Task GetUsersAllAnnouncements_WhenAnnouncementsExist_ReturnsMappedAnnouncements()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcements = new List<Announcement>
            {
                new Announcement { Id = Guid.NewGuid(), Title = "Test 1" },
                new Announcement { Id = Guid.NewGuid(), Title = "Test 2" }
            };

            _announcementRepositoryMock
                .Setup(r => r.GetAllAsync(userId, true, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcements);

            // Act
            var result = await _userProfileServiceMock.GetUsersAllAnnouncements(userId, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(announcements[0].Title, result[0].Title);
            Assert.Equal(announcements[1].Title, result[1].Title);
            _announcementRepositoryMock.Verify(r => r.GetAllAsync(userId, true, null, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region GetUsersAllAnnouncements_WhenNoAnnouncements_ReturnsEmptyList
        [Fact]
        public async Task GetUsersAllAnnouncements_WhenNoAnnouncements_ReturnsEmptyList()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _announcementRepositoryMock
                .Setup(r => r.GetAllAsync(userId, false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Announcement>());

            // Act
            var result = await _userProfileServiceMock.GetUsersAllAnnouncements(userId, false);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _announcementRepositoryMock.Verify(r => r.GetAllAsync(userId, false, null, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion
    }
}
