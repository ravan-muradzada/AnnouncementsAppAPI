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
    public class DeleteAnnouncementTest : AdminServiceTestBase
    {
        #region AdminService_Success_ThrowsObjectNotFoundException
        [Fact]
        public async Task AdminService_Success_DeletesAnnouncement()
        {
            // Arrange
            var announcement = new Announcement
            {
                Id = Guid.NewGuid(),
                Title = "Test Announcement",
                Content = "This is a test announcement.",
                Author = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    Email = "some@gmail.com"
                }
            };
            _announcementRepositoryMock
                .Setup(ar => ar.GetByIdAsync(It.IsAny<Guid>(), null, false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcement);

            _announcementRepositoryMock
                .Setup(ar => ar.DeleteAsync(It.IsAny<Announcement>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            _emailServiceMock
                .Setup(es => es.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

            // Act
            var exception = await Record.ExceptionAsync(() =>
                _adminService.DeleteAnnouncement(announcement.Id));

            // Assert
            Assert.Null(exception);
        }
        #endregion

        #region AdminService_Failure_ThrowsObjectNotFoundException
        [Fact]
        public async Task AdminService_Failure_ThrowsObjectNotFoundException()
        {
            // Arrange
            _announcementRepositoryMock
                .Setup(ar => ar.GetByIdAsync(It.IsAny<Guid>(), null, false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Announcement?)null);
            // Act & Assert
            await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _adminService.DeleteAnnouncement(Guid.NewGuid()));
        }
        #endregion
    }
}
