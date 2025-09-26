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
        public async Task JoinAnnouncementAsync(AnnouncementUser announcementUser)
        {
            await _dbContext.AnnouncementUsers.AddAsync(announcementUser);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region LeaveAnnouncementAsync
        public async Task LeaveAnnouncementAsync(AnnouncementUser announcementUser)
        {
            _dbContext.AnnouncementUsers.Remove(announcementUser);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        #region CheckJoin
        public async Task<bool> CheckJoin(Guid announcementId, Guid userId)
        {
            var exists = await _dbContext.AnnouncementUsers
                .FirstOrDefaultAsync(au => au.AnnouncementId == announcementId && au.ApplicationUserId == userId);
            return exists is not null;
        }
        #endregion

        #region GetAnnouncementUserAsync
        public async Task<AnnouncementUser?> GetAnnouncementUserAsync(Guid announcementId, Guid userId)
        {
            return await _dbContext.AnnouncementUsers.FirstOrDefaultAsync(au => au.AnnouncementId == announcementId && au.ApplicationUserId == userId);
        }
        #endregion

        #region GetAnnouncementUsersAsync
        public async Task<List<AnnouncementUser>> GetAnnouncementUsers(Guid announcementId)
        {
            return await _dbContext.AnnouncementUsers
                .Where(au => au.AnnouncementId == announcementId)
                .Include(au => au.User.UserName)
                .ToListAsync();
        }
        #endregion

        #region DisjoinUserFromAnnouncement
        public async Task DisjoinUserFromAnnouncement(AnnouncementUser announcementUser)
        {
            _dbContext.AnnouncementUsers.Remove(announcementUser);
            await _dbContext.SaveChangesAsync();
        }
        #endregion
    }
}
