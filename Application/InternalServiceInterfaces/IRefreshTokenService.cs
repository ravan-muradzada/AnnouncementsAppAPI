using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServiceInterfaces
{
    public interface IRefreshTokenService
    {
        Task<string> GenerateRefreshToken(Guid userId);
        Task<ApplicationUser?> FindUserOfRefreshTokenAndDeleteToken(string refreshToken);
        Task InvalidateUserTokensAsync(Guid userId);
    }
}
