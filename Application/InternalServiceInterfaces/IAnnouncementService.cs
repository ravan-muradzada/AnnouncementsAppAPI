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
        Task<AnnouncementResponse> CreateAnnouncement(Guid userId, CreateAnnouncementRequest request, CancellationToken ct = default);
        Task<AnnouncementResponse> GetAnnouncement(Guid announcemenetId, CancellationToken ct = default);
        Task<AnnouncementResponse> UpdateAnnouncement(Guid userId, Guid announcementId, UpdateAnnouncementRequest request, CancellationToken ct = default);
        Task DeleteAnnouncement(Guid userId, Guid announcementId, CancellationToken ct);
        Task<List<AnnouncementResponse>> GetAllAnnouncements(CancellationToken ct);
        Task<PagedResult<AnnouncementResponse>> GetPagedAnnouncements(
            int page, 
            int pageSize, 
            string? search = null, 
            string? category = null, 
            bool? isPublished = null, 
            bool? isPinned = null, 
            CancellationToken ct = default);
    }
}
