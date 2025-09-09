using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public Guid RefreshTokenId { get; set; }
        public string Token { get; set; } = default!;
        public DateTime ExpirationTime { get; set; } = default!;
        public Guid ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; } = default!;
    }
}
