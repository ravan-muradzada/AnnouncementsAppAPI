using Domain.Common;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IAnnouncementRepository
    {
        Task<Announcement> AddAync(Announcement announcement, CancellationToken ct = default);
        Task<Announcement?> GetByIdAsync(Guid announcementId, Guid? userId = null, bool? isPublished = null, CancellationToken ct = default);
        Task<List<Announcement>> GetAllAsync(Guid? userId = null, bool? isPublished = null, CancellationToken ct = default);
        Task<PagedResult<Announcement>> GetPagedAsync(
                int page, int pageSize, Guid? userId = null ,bool? isPublished = null,
                string? search = null,
                string? category = null,
                bool? isPinned = null,
                CancellationToken ct = default);
        Task<Announcement> UpdateAsync(Announcement announcement, bool isPublished, CancellationToken ct = default);
        Task DeleteAsync(Announcement announcement, CancellationToken ct = default);
        Task SaveChangesAsync(Announcement announcement, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid announcementId);
    }
}
