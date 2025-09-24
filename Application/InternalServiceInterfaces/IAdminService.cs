using Application.DTOs.Announcement.Response;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces
{
    public interface IAdminService
    {
        Task<List<AnnouncementResponse>> GetAllAnnouncements(CancellationToken ct = default);
        Task<AnnouncementResponse> GetAnnouncementById(Guid announcementId, CancellationToken ct = default);
        Task<PagedResult<AnnouncementResponse>> GetAnnouncements(int pageNumber, int pageSize, Guid? userId = null, string? search = null, string? category = null, bool? isPinned = null, CancellationToken ct = default);
        Task PublishAnnouncement(Guid announcementId, CancellationToken ct = default);
        Task DeleteAnnouncement(Guid announcementId, CancellationToken ct = default);
    }
}
