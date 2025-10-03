using Domain.Common;
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
    public class GetPagedAnnouncementsTest : AnnouncementServiceTestBase
    {
        #region GetPagedAnnouncements_ValidParameters_ReturnsPagedResult
        [Fact]
        public async Task GetPagedAnnouncements_ValidParameters_ReturnsPagedResult()
        {
            // Arrange
            var announcements = new List<Announcement>
            {
                new Announcement { Title = "title-1", Content = "content-1" },
                new Announcement { Title = "title-2", Content = "content-2" },
                new Announcement { Title = "title-3", Content = "content-3" },
                new Announcement { Title = "title-4", Content = "content-4" },
                new Announcement { Title = "title-5", Content = "content-5" },
                new Announcement { Title = "title-6", Content = "content-6" },
                new Announcement { Title = "title-7", Content = "content-7" },
                new Announcement { Title = "title-8", Content = "content-8" }
            };

            _announcementRepositoryMock.Setup(repo => repo.GetPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<bool?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Announcement> { Items = announcements });

            int page = 1;
            int pageSize = 2;
            string? search = "Announcement";
            string? category = "General";
            bool? isPinned = null;
            // Act
            var result = await _announcementService.GetPagedAnnouncements(page, pageSize, search, category, isPinned);
            // Assert
            Assert.NotNull(result);
            Assert.Equal(8, result.Items.Count);
        }
        #endregion

        #region GetPagedAnnouncements_EmptyResult_ReturnsEmptyPagedResult
        [Fact]
        public async Task GetPagedAnnouncements_EmptyResult_ReturnsEmptyPagedResult()
        {
            // Arrange
            var emptyAnnouncements = new List<Announcement>();
            _announcementRepositoryMock.Setup(repo => repo.GetPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<bool?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Announcement> { Items = emptyAnnouncements });

            int page = 1;
            int pageSize = 10;
            string? search = null;
            string? category = null;
            bool? isPinned = null;

            // Act
            var result = await _announcementService.GetPagedAnnouncements(page, pageSize, search, category, isPinned);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }
        #endregion

        #region GetPagedAnnouncements_FilterByCategory_ReturnsFilteredResult
        [Fact]
        public async Task GetPagedAnnouncements_FilterByCategory_ReturnsFilteredResult()
        {
            // Arrange
            var announcements = new List<Announcement>
    {
        new Announcement { Title = "title-1", Content = "content-1", Category = "General" },
        new Announcement { Title = "title-2", Content = "content-2", Category = "Updates" },
        new Announcement { Title = "title-3", Content = "content-3", Category = "General" }
    };

            _announcementRepositoryMock.Setup(repo => repo.GetPagedAsync(
                It.IsAny<int>(),
                It.IsAny<int>(),
                It.IsAny<Guid?>(),
                It.IsAny<bool?>(),
                It.IsAny<string?>(),
                It.IsAny<string?>(),
                It.IsAny<bool?>(),
                It.IsAny<bool?>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PagedResult<Announcement> { Items = announcements.Where(a => a.Category == "General").ToList() });

            int page = 1;
            int pageSize = 10;
            string? search = null;
            string? category = "General";
            bool? isPinned = null;

            // Act
            var result = await _announcementService.GetPagedAnnouncements(page, pageSize, search, category, isPinned);

            // Assert
            Assert.NotNull(result);
            Assert.All(result.Items, a => Assert.Equal("General", a.Category));
        }
        #endregion
    }
}
