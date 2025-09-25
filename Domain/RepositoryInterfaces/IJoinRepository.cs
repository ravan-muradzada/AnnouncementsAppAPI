using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IJoinRepository
    {
        Task JoinAnnouncementAsync(AnnouncementUser announcementUser);
        Task LeaveAnnouncementAsync(AnnouncementUser announcementUser);
        Task<bool> CheckJoin(Guid announcementId, Guid userId);  
        Task<AnnouncementUser?> GetAnnouncementUserAsync(Guid announcementId, Guid userId);
    }
}
