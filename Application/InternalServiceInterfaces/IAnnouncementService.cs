using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces
{
    public interface IAnnouncementService
    {
        Task<AnnouncementResponse> GetAnnouncement(Guid announcemenetId, CancellationToken ct = default);
        Task<List<AnnouncementResponse>> GetAllAnnouncements(CancellationToken ct = default);
        Task<PagedResult<AnnouncementResponse>> GetPagedAnnouncements(
            int page, 
            int pageSize, 
            string? search = null, 
            string? category = null, 
            bool? isPinned = null, 
            CancellationToken ct = default);
    }
}
