using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public readonly int countOfAllowedChangeUsername = 3;
        public int currentCountOfChange { get; set; } = 0;
        public DateTime? LimitDate { get; set; }
        public List<Announcement> Announcements { get; set; } = new List<Announcement>();
        public List<AnnouncementUser> JoinedAnnouncements { get; set; } = new List<AnnouncementUser>();
    }
}
