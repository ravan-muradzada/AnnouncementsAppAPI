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
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public readonly int countOfAllowedChangeUsername = 3;
        public int currentCountOfChange { get; set; } = 0;
        public DateTime? LimitDate { get; set; }
    }
}
