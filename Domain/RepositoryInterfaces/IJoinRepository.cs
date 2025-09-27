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
        Task JoinAnnouncementAsync(AnnouncementUser announcementUser, CancellationToken ct = default);
        Task LeaveAnnouncementAsync(AnnouncementUser announcementUser, CancellationToken ct = default);
        Task<bool> CheckJoin(Guid announcementId, Guid userId, CancellationToken ct = default);  
        Task<AnnouncementUser?> GetAnnouncementUserAsync(Guid announcementId, Guid userId, CancellationToken ct = default);
        Task<List<AnnouncementUser>> GetAnnouncementUsers(Guid announcementId, CancellationToken ct = default);
        Task DisjoinUserFromAnnouncement(AnnouncementUser announcementUser, CancellationToken ct = default);
    }
}
