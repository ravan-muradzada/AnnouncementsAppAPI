using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.RepositoryInterfaces
{
    public interface IRefreshTokenRepository
    {
        Task AddAsync(RefreshToken refreshTokenObject);
        Task<RefreshToken?> GetAsync(string refreshToken);
        Task DeleteAsync(RefreshToken refreshTokenObject);
        Task DeleteAllByUserIdAsync(Guid userId);
    }
}
