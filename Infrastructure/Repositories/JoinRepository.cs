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
    public class JoinRepository : IJoinRepository
    {
        #region Fields
        private readonly ApplicationDbContext _dbContext;
        #endregion

        #region Constructor
        public JoinRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region JoinAnnouncementAsync
        public async Task JoinAnnouncementAsync(AnnouncementUser announcementUser, CancellationToken ct = default)
        {
            await _dbContext.AnnouncementUsers.AddAsync(announcementUser, ct);
            await _dbContext.SaveChangesAsync(ct);
        }
        #endregion

        #region LeaveAnnouncementAsync
        public async Task LeaveAnnouncementAsync(AnnouncementUser announcementUser, CancellationToken ct = default)
        {
            _dbContext.AnnouncementUsers.Remove(announcementUser);
            await _dbContext.SaveChangesAsync(ct);
        }
        #endregion

        #region CheckJoin
        public async Task<bool> CheckJoin(Guid announcementId, Guid userId, CancellationToken ct = default)
        {
            var exists = await _dbContext.AnnouncementUsers
                .FirstOrDefaultAsync(au => au.AnnouncementId == announcementId && au.ApplicationUserId == userId, ct);
            return exists is not null;
        }
        #endregion

        #region GetAnnouncementUserAsync
        public async Task<AnnouncementUser?> GetAnnouncementUserAsync(Guid announcementId, Guid userId, CancellationToken ct = default)
        {
            return await _dbContext.AnnouncementUsers.FirstOrDefaultAsync(au => au.AnnouncementId == announcementId && au.ApplicationUserId == userId, ct);
        }
        #endregion

        #region GetAnnouncementUsersAsync
        public async Task<List<AnnouncementUser>> GetAnnouncementUsers(Guid announcementId, CancellationToken ct = default)
        {
            return await _dbContext.AnnouncementUsers
                .Where(au => au.AnnouncementId == announcementId)
                .Include(au => au.User.UserName)
                .ToListAsync(ct);
        }
        #endregion

        #region DisjoinUserFromAnnouncement
        public async Task DisjoinUserFromAnnouncement(AnnouncementUser announcementUser, CancellationToken ct = default)
        {
            _dbContext.AnnouncementUsers.Remove(announcementUser);
            await _dbContext.SaveChangesAsync(ct);
        }
        #endregion
    }
}
