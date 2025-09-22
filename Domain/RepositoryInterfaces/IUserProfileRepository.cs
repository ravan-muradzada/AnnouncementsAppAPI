using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IUserProfileRepository
    {
        Task<List<Announcement>> GetAllAnnouncementsByUserIdAsync(Guid userId, bool isPublished, CancellationToken ct = default);
        Task<PagedResult<Announcement>> GetPagedAnnouncementsByUserIdAsync(Guid userId, int page, int pageSize, bool isPublished ,string? search = null, string? category = null, bool? isPinned = null, CancellationToken ct = default);
        Task<Announcement?> GetAnnouncementByIdAndUserIdAsync(Guid announcementId, Guid userId, bool isPublished, CancellationToken ct = default);
    }
}
