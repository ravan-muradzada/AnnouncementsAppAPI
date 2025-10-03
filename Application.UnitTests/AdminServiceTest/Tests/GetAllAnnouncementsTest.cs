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
    public class GetAllAnnouncementsTest : AdminServiceTestBase
    {
        #region GetAllAnnouncements_Success
        [Fact]
        public async Task GetAllAnnouncements_Success()
        {
            _announcementRepositoryMock.Setup(repo => repo.GetAllAsync(
                null,
                false,
                null,
                default))
                .ReturnsAsync(new List<Announcement>
                {
                    new Announcement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Announcement 1",
                        Content = "Content 1",
                        AuthorId = Guid.NewGuid(),
                        IsPublished = true,
                        PublishedAt = DateTime.UtcNow,
                        ExpiresAt = DateTime.UtcNow.AddDays(10),
                        Category = "General",
                        IsPinned = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    },
                    new Announcement
                    {
                        Id = Guid.NewGuid(),
                        Title = "Announcement 2",
                        Content = "Content 2",
                        AuthorId = Guid.NewGuid(),
                        IsPublished = false,
                        PublishedAt = null,
                        ExpiresAt = DateTime.UtcNow.AddDays(5),
                        Category = "Updates",
                        IsPinned = true,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    }
                });
            
            var result = await _adminService.GetAllAnnouncements();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
        #endregion
    }
}
