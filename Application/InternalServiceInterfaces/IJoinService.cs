using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces
{
    public interface IJoinService
    {
        Task JoinAnnouncementAsync(Guid announcementId, Guid userId);
        Task LeaveAnnouncementAsync(Guid announcementId, Guid userId);
    }
}
