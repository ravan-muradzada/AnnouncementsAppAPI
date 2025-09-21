using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Announcement.Request
{
    public class CreateAnnouncementRequest
    {
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public string Category { get; set; } = default!;
        public DateTime? ExpiresAt { get; set; }
    }
}
