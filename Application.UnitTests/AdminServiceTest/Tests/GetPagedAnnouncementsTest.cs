using Domain.Common;
using Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Application.UnitTests.AdminServiceTest.Tests
{
    public class GetPagedAnnouncementsTest : AdminServiceTestBase
    {
        #region GetAnnouncements_ReturnsPagedResult_WithMappedItems
        [Fact]
        public async Task GetAnnouncements_ReturnsPagedResult_WithMappedItems()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement { Id = Guid.NewGuid(), Title = "Test 1", Content = "Content 1" },
                new Announcement { Id = Guid.NewGuid(), Title = "Test 2", Content = "Content 2" }
            };

            var pagedResult = new PagedResult<Announcement>
            {
                Items = announcements,
                TotalCount = 2,
                PageSize = 2,
                CurrentPageNumber = 1
            };

            _announcementRepositoryMock
                .Setup(r => r.GetPagedAsync(1, 2, null, false, null, null, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _adminService.GetAnnouncements(1, 2);

            // Assert
            Assert.Equal(2, result.Items.Count);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(1, result.CurrentPageNumber);
            Assert.Contains(result.Items, r => r.Title == "Test 1");
            Assert.Contains(result.Items, r => r.Title == "Test 2");
        }
        #endregion

        #region GetAnnouncements_ReturnsEmpty_WhenNoAnnouncements
        [Fact]
        public async Task GetAnnouncements_ReturnsEmpty_WhenNoAnnouncements()
        {
            // Arrange
            var pagedResult = new PagedResult<Announcement>
            {
                Items = new List<Announcement>(),
                TotalCount = 0,
                PageSize = 2,
                CurrentPageNumber = 1
            };

            _announcementRepositoryMock
                .Setup(r => r.GetPagedAsync(1, 2, null, false, null, null, null, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pagedResult);

            // Act
            var result = await _adminService.GetAnnouncements(1, 2);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(0, result.TotalCount);
            Assert.Equal(2, result.PageSize);
            Assert.Equal(1, result.CurrentPageNumber);
        }
        #endregion
    }
}
