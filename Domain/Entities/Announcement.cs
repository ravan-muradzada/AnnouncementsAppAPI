using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Announcement
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public Guid AuthorId { get; set; }
        public ApplicationUser Author { get; set; } = null!;
        public bool IsPublished { get; set; }
        public DateTime PublishedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string Category { get; set; } = null!;
        public bool isPinned { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
