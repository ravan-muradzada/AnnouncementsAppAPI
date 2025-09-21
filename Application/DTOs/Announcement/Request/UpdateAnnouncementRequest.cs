using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Announcement.Request
{
    public class UpdateAnnouncementRequest
    {
        public string? Title { get; set; }
        public string? Content { get; set; } 
        public string? Category { get; set; }
        public DateTime? ExpiresAt { get; set; } 
    }
}
