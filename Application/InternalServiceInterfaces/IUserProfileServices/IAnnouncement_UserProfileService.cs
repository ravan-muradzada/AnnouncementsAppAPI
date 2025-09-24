using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces.IUserProfileServices
{
    public interface IAnnouncement_UserProfileService
    {
        Task<AnnouncementResponse> CreateAnnouncement(Guid userId, CreateAnnouncementRequest request, CancellationToken ct = default);
        Task<AnnouncementResponse> UpdateAnnouncement(Guid userId, Guid announcementId, UpdateAnnouncementRequest request, CancellationToken ct = default);
        Task DeleteAnnouncement(Guid userId, Guid announcementId, CancellationToken ct);
        Task<List<AnnouncementResponse>> GetUsersAllAnnouncements(Guid userId, bool isPublished, CancellationToken ct = default);
        Task<PagedResult<AnnouncementResponse>> GetUsersPagedAnnouncements(Guid userId, int page,
            int pageSize,
            bool isPublished,
            string? search = null,
            string? category = null,
            bool? isPinned = null,
            CancellationToken ct = default);
        Task<AnnouncementResponse> GetAnnouncementById(Guid userId, Guid announcementId, CancellationToken ct = default);
    }
}
