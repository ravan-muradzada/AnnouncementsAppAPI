using Domain.CustomExceptions;
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
    public class PublishAnnouncementTest : AdminServiceTestBase
    {
        #region PublishAnnouncement_Success
        [Fact]
        public async Task PublishAnnouncement_Success()
        {
            Announcement announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = "Test Announcement",
                Content = "This is a test announcement.",
                Author = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = "mail@gmail.com"
                }
            };
            _announcementRepositoryMock.Setup(repo => repo.GetByIdAsync(announcement.Id, null, false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcement);
            _announcementRepositoryMock.Setup(repo => repo.SaveChangesAsync(announcement, It.IsAny<CancellationToken>()));
            _emailServiceMock.Setup(email => email.SendEmail(
                announcement.Author.Email!,
                "Announcement Published",
                $"Your announcement titled '{announcement.Title}' has been published."))
                .Returns(Task.CompletedTask);

            var exception = await Record.ExceptionAsync(() => _adminService.PublishAnnouncement(announcement.Id));
            Assert.Null(exception);
            Assert.True(announcement.IsPublished);
            Assert.NotNull(announcement.PublishedAt);
            Assert.Equal(DateTime.UtcNow.Date, announcement.PublishedAt?.Date);

            _announcementRepositoryMock.Verify(repo => repo.GetByIdAsync(announcement.Id, null, false, null, It.IsAny<CancellationToken>()), Times.Once);
            _announcementRepositoryMock.Verify(repo => repo.SaveChangesAsync(announcement, It.IsAny<CancellationToken>()), Times.Once);
            _emailServiceMock.Verify(email => email.SendEmail(
                announcement.Author.Email!,
                "Announcement Published",
                $"Your announcement titled '{announcement.Title}' has been published."), Times.Once);
        }
        #endregion

        #region PublishAnnouncement_AnnouncementNotFound
        [Fact]
        public async Task PublishAnnouncement_AnnouncementNotFound()
        {
            Guid announcementId = Guid.NewGuid();
            _announcementRepositoryMock.Setup(repo => repo.GetByIdAsync(announcementId, null, false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Announcement?)null);

            var exception = await Assert.ThrowsAsync<ObjectNotFoundException>(() => _adminService.PublishAnnouncement(announcementId));
            Assert.Equal("Announcement not found", exception.Message);

            _announcementRepositoryMock.Verify(repo => repo.GetByIdAsync(announcementId, null, false, null, It.IsAny<CancellationToken>()), Times.Once);
            _announcementRepositoryMock.Verify(repo => repo.SaveChangesAsync(It.IsAny<Announcement>(), It.IsAny<CancellationToken>()), Times.Never);
            _emailServiceMock.Verify(email => email.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
        #endregion
    }
}
