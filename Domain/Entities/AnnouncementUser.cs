using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class AnnouncementUser
    {
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid AnnouncementId { get; set; }
        public Announcement Announcement { get; set; } = null!;
        public DateTime JoinedAt = DateTime.UtcNow;
    }
}
