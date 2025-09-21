using Domain.Common;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AnnouncementRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Announcement> AddAync(Announcement announcement, CancellationToken ct = default)
        {
            await _dbContext.Announcements.AddAsync(announcement, ct);
            await _dbContext.SaveChangesAsync();
            return announcement;
        }

        public async Task DeleteAsync(Announcement announcement, CancellationToken ct = default)
        {
            _dbContext.Announcements.Remove(announcement);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            Announcement? announcement = await _dbContext.Announcements.FirstOrDefaultAsync(x => x.Id == id);
            return announcement is not null;
        }

        public async Task<List<Announcement>> GetAllAsync(CancellationToken ct = default)
        {
            List<Announcement> announcements = await _dbContext.Announcements.ToListAsync();
            return announcements;
        }

        public async Task<Announcement?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            Announcement? announcement = await _dbContext.Announcements.FirstOrDefaultAsync(a => a.Id == id);
            return announcement;
        }

        public async Task<PagedResult<Announcement>> GetPagedAsync(int page, int pageSize, string? search = null, string? category = null, bool? isPublished = null, bool? isPinned = null, CancellationToken ct = default)
        {
            var announcements = _dbContext.Announcements.AsQueryable();

            if (search is not null)
            {
                announcements = (IQueryable<Announcement>)announcements.Where(a => a.Title.Contains(search) || a.Content.Contains(search)).ToList();
            }

            if (category is not null)
            {
                announcements = (IQueryable<Announcement>)announcements.Where(a => a.Category == category).ToList();
            }

            if (isPublished.HasValue)
            {
                announcements = (IQueryable<Announcement>)announcements.Where(a => a.IsPublished == isPublished).ToList();
            }

            if (isPinned.HasValue)
            {
                announcements = (IQueryable<Announcement>)announcements.Where(a => a.isPinned == isPinned).ToList();
            }

            var totalCount = await announcements.CountAsync(ct);

            var items = await announcements
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PagedResult<Announcement>(items, totalCount, page, pageSize);
        }

        public async Task SaveChangesAsync(Announcement announcement, CancellationToken ct = default)
        {
            _dbContext.Announcements.Update(announcement);
            await _dbContext.SaveChangesAsync(ct);
        }

        public async Task<Announcement> UpdateAsync(Announcement announcement, CancellationToken ct = default)
        {
            _dbContext.Announcements.Update(announcement);
            await _dbContext.SaveChangesAsync();
            return announcement;
        }
    }
}
