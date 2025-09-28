using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Announcement.Response
{
    public sealed record AnnouncementResponse(Guid Id, string Title, string Content, string Category, bool IsExpired, DateTime? ExpiresAt, DateTime? PublishedAt, bool IsPublished, bool IsPinned);
}
