using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Announcement.Response
{
    public class AnnouncementResponse
    {
        Guid Id { get; set; } = default!;
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string Category { get; set; } = default!;
        public DateTime? ExpiresAt { get; set; }
        public DateTime? PublishedAt { get; set; } 
        public bool isPublished { get; set; } = default!;
        public bool isPinned { get; set; } = default!;
    }
}
