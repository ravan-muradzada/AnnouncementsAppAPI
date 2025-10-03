using Domain.Common;
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
    public class GetUsersPagedAnnouncementsTest : Announcement_UserProfileTestBase
    {
        #region GetUsersPagedAnnouncements_WhenAnnouncementsExist_ReturnsPagedResult
        [Fact]
        public async Task GetUsersPagedAnnouncements_WhenAnnouncementsExist_ReturnsPagedResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcements = new PagedResult<Announcement>
            {
                Items = new List<Announcement>
        {
            new Announcement { Id = Guid.NewGuid(), Title = "Test 1" },
            new Announcement { Id = Guid.NewGuid(), Title = "Test 2" }
        },
                TotalCount = 2,
                PageSize = 10,
                CurrentPageNumber = 1
            };

            _announcementRepositoryMock
                .Setup(r => r.GetPagedAsync(1, 10, userId, true, null, null, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcements);

            // Act
            var result = await _userProfileServiceMock.GetUsersPagedAnnouncements(userId, 1, 10, true);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.CurrentPageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal("Test 1", result.Items[0].Title);
            Assert.Equal("Test 2", result.Items[1].Title);
            _announcementRepositoryMock.Verify(r =>
                r.GetPagedAsync(1, 10, userId, true, null, null, null, null, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion

        #region GetUsersPagedAnnouncements_WhenNoAnnouncements_ReturnsEmptyPagedResult
        [Fact]
        public async Task GetUsersPagedAnnouncements_WhenNoAnnouncements_ReturnsEmptyPagedResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var announcements = new PagedResult<Announcement>
            {
                Items = new List<Announcement>(),
                TotalCount = 0,
                PageSize = 10,
                CurrentPageNumber = 1
            };

            _announcementRepositoryMock
                .Setup(r => r.GetPagedAsync(1, 10, userId, false, null, null, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcements);

            // Act
            var result = await _userProfileServiceMock.GetUsersPagedAnnouncements(userId, 1, 10, false);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
            _announcementRepositoryMock.Verify(r =>
                r.GetPagedAsync(1, 10, userId, false, null, null, null, null, It.IsAny<CancellationToken>()), Times.Once);
        }
        #endregion
    }
}
