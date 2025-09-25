using Domain.Common;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        #region Fields
        private readonly ApplicationDbContext _dbContext;
        #endregion

        #region Constructor
        public AnnouncementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region AddAsync
        public async Task<Announcement> AddAync(Announcement announcement, CancellationToken ct = default)
        {
            await _dbContext.Announcements.AddAsync(announcement, ct);
            await _dbContext.SaveChangesAsync();
            return announcement;
        }
        #endregion

        #region DeleteAsync
        public async Task DeleteAsync(Announcement announcement, CancellationToken ct = default)
        {
            _dbContext.Announcements.Remove(announcement);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region GetAllAsync
        public async Task<List<Announcement>> GetAllAsync(Guid? userId = null, bool? isPublished = null, CancellationToken ct = default)
        {
            var query = _dbContext.Announcements.AsQueryable();

            if (userId is not null)
            {
                query = query.Where(a => a.AuthorId == userId);
            }

            if (isPublished is not null)
            {
                query = query.Where(a => a.IsPublished == isPublished);
            }

            return await query.
                OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
        #endregion

        #region GetByIdAsync
        public async Task<Announcement?> GetByIdAsync(Guid announcementId, Guid? userId = null, bool? isPublished = null, CancellationToken ct = default)
        {
            var query = _dbContext.Announcements.AsQueryable();

            query = query.Where(a => a.Id == announcementId);

            if (userId is not null)
            {
                query = query.Where(a => a.AuthorId == userId);
            }

            if (isPublished is not null)
            {
                query = query.Where(a => a.IsPublished == isPublished);
            }
            return await query.FirstOrDefaultAsync(ct);
        }
        #endregion

        #region GetPagedAsync
        public async Task<PagedResult<Announcement>> GetPagedAsync(int page, int pageSize, Guid? userId = null, bool? isPublished = null, string? search = null, string? category = null, bool? isPinned = null, CancellationToken ct = default)
        {
            var announcements = _dbContext.Announcements.AsQueryable();


            if (userId is not null)
            {
                announcements = announcements.Where(a => a.AuthorId == userId);
            }

            if (isPublished is not null)
            {
                announcements = announcements.Where(a => a.IsPublished == isPublished);
            }

            if (search is not null)
            {
                announcements = announcements.Where(a => a.Title.Contains(search) || a.Content.Contains(search));
            }

            if (category is not null)
            {
                announcements = announcements.Where(a => a.Category == category);
            }

            if (isPinned.HasValue)
            {
                announcements = announcements.Where(a => a.IsPinned == isPinned);
            }

            var totalCount = await announcements.CountAsync(ct);

            var items = await announcements
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync(ct);

            return new PagedResult<Announcement>(items, totalCount, page, pageSize);
        }
        #endregion

        #region SaveChangesAsync
        public async Task SaveChangesAsync(Announcement announcement, CancellationToken ct = default)
        {
            _dbContext.Announcements.Update(announcement);
            await _dbContext.SaveChangesAsync(ct);
        }
        #endregion

        #region UpdateAsync
        public async Task<Announcement> UpdateAsync(Announcement announcement, bool isPublished ,CancellationToken ct = default)
        {
            _dbContext.Announcements.Update(announcement);
            announcement.UpdatedAt = DateTime.UtcNow;
            announcement.IsPublished = isPublished;
            await _dbContext.SaveChangesAsync();
            return announcement;
        }
        #endregion

        #region ExistsAsync
        public async Task<bool> ExistsAsync(Guid announcementId)
        {
            Announcement? announcement = await _dbContext.Announcements.FirstOrDefaultAsync(a => a.Id == announcementId);
            return announcement is not null;
        }
        #endregion
    }
}
