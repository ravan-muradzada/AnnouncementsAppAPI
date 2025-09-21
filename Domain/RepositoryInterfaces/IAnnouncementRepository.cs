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
        Task<Announcement?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<Announcement>> GetAllAsync(CancellationToken ct = default);
        Task<PagedResult<Announcement>> GetPagedAsync(
                int page, int pageSize,
                string? search = null,
                string? category = null,
                bool? isPublished = null,
                bool? isPinned = null,
                CancellationToken ct = default);
        Task<Announcement> UpdateAsync(Announcement announcement, CancellationToken ct = default);
        Task DeleteAsync(Announcement announcement, CancellationToken ct = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken ct = default);
        Task SaveChangesAsync(Announcement announcement ,CancellationToken ct = default);
    }
}
