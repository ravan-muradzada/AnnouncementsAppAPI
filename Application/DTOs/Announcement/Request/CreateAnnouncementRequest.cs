using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Announcement.Request
{
    public sealed record CreateAnnouncementRequest
        (string Title, string Content, string Category, DateTime? ExpiresAt);
}
