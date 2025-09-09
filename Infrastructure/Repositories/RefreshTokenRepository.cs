using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RefreshTokenRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(RefreshToken refreshTokenObject)
        {
            await _dbContext.RefreshTokens.AddAsync(refreshTokenObject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAllByUserIdAsync(Guid userId)
        {
            var refreshTokens = _dbContext.RefreshTokens.Where(rt => rt.ApplicationUserId == userId);
            _dbContext.RefreshTokens.RemoveRange(refreshTokens);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(RefreshToken refreshTokenObject)
        {
            _dbContext.RefreshTokens.Remove(refreshTokenObject);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetAsync(string refreshToken)
        {
            RefreshToken? refreshTokenObject = await _dbContext.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            return refreshTokenObject;
        }
    }

}
