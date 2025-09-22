using Domain.Common;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        #region Fields
        private readonly ApplicationDbContext _dbContext;
        #endregion

        #region Constructor
        public UserProfileRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region GetAllAnnouncementsByUserIdAsync
        public async Task<List<Announcement>> GetAllAnnouncementsByUserIdAsync(Guid userId, bool isPublished, CancellationToken ct = default)
        {
            return await _dbContext.Announcements
                .Where(announcement => announcement.AuthorId == userId && announcement.IsPublished == isPublished)
                .OrderByDescending(announcement => announcement.CreatedAt)
                .ToListAsync(ct);
        }
        #endregion

        #region GetAnnouncementByIdAndUserIdAsync
        public async Task<Announcement?> GetAnnouncementByIdAndUserIdAsync(Guid announcementId, Guid userId, bool isPublished, CancellationToken ct = default)
        {
            return await _dbContext.Announcements
                .FirstOrDefaultAsync(a => a.Id == announcementId && a.AuthorId == userId && a.IsPublished == isPublished, ct);
        }
        #endregion

        #region GetPagedAnnouncementsByUserIdAsync
        public async Task<PagedResult<Announcement>> GetPagedAnnouncementsByUserIdAsync(Guid userId, int page, int pageSize, bool isPublished, string? search = null, string? category = null, bool? isPinned = null, CancellationToken ct = default)
        {
            var query = _dbContext.Announcements.AsQueryable();

            query = query.Where(a => a.IsPublished == isPublished && a.AuthorId == userId);

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(a => a.Title.Contains(search) || a.Content.Contains(search));
            }

            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(a => a.Category == category);
            }

            if (isPinned.HasValue)
            {
                query = query.Where(a => a.IsPinned == isPinned.Value);
            }

            var totalCount = await query.CountAsync(ct);

            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);
            return new PagedResult<Announcement>(items, totalCount, page, pageSize);
        }
        #endregion
    }
}
