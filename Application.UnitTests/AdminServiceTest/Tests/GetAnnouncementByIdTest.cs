using Application.DTOs.Announcement.Response;
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
    public class GetAnnouncementByIdTest : AdminServiceTestBase
    {
        #region GetAnnouncementById_ReturnsMappedResponse_WhenAnnouncementExists
        [Fact]
        public async Task GetAnnouncementById_ReturnsMappedResponse_WhenAnnouncementExists()
        {
            // Arrange
            var announcementId = Guid.NewGuid();
            var announcement = new Announcement
            {
                Id = announcementId,
                Title = "Test Title",
                Content = "Test Content"
            };

            var mappedResponse = new AnnouncementResponse(
                announcementId, "Test Title", "Test Content", "Category", false, null, null, false, false
            );

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, null, false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync(announcement);

            // Act
            var result = await _adminService.GetAnnouncementById(announcementId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(mappedResponse.Id, result.Id);
            Assert.Equal(mappedResponse.Title, result.Title);
        }
        #endregion

        #region GetAnnouncementById_ThrowsObjectNotFoundException_WhenAnnouncementDoesNotExist
        [Fact]
        public async Task GetAnnouncementById_ThrowsObjectNotFoundException_WhenAnnouncementDoesNotExist()
        {
            // Arrange
            var announcementId = Guid.NewGuid();

            _announcementRepositoryMock
                .Setup(r => r.GetByIdAsync(announcementId, null, false, null, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Announcement?)null);

            // Act
            var ex = await Assert.ThrowsAsync<ObjectNotFoundException>(() =>
                _adminService.GetAnnouncementById(announcementId));

            // Assert
            Assert.Equal("Announcement not found", ex.Message);
        }
        #endregion
    }
}
