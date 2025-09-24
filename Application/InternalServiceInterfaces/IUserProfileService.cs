using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Application.DTOs.UserProfile.Request;
using Application.DTOs.UserProfile.Response;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces
{
    public interface IUserProfileService
    {
        Task<UserProfileResponse> GetUser(Guid userId);
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
        Task<UserProfileResponse> ChangeUsername(Guid userId, ChangeUsernameRequest request);
        Task ChangeEmail(Guid userId, ChangeEmailRequest request);
        Task<UserProfileResponse> VerifyEmailChange(Guid userId, VerifyEmailChangeRequest request);
        Task ChangePassword(Guid userId, ChangePasswordRequest request);
        Task EnableTwoFactorAuth(Guid userId);
        Task DisableTwoFactorAuth(Guid userId);
    }
}
