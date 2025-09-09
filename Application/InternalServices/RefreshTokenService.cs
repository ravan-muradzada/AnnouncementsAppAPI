using Application.InternalServiceInterfaces;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.InternalServices
{
    public class RefreshTokenService : IRefreshTokenService
    {
        #region Fields
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository, IConfiguration configuration)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _configuration = configuration;
        }
        #endregion

        #region GenerateRefreshToken
        public async Task<RefreshToken> GenerateRefreshToken(Guid userId)
        {
            byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            string refreshToken = Convert.ToBase64String(bytes);
            var expirationTime = Convert.ToDouble(_configuration["RefreshToken:EXPIRATION_MINUTES"] ?? "10");
            RefreshToken refreshTokenObject = new RefreshToken { Token = refreshToken, ApplicationUserId = userId, ExpirationTime = DateTime.UtcNow.AddMinutes(expirationTime) };

            await _refreshTokenRepository.AddAsync(refreshTokenObject);

            return refreshTokenObject;
        }
        #endregion

        #region FindUserOfRefreshTokenAndDeleteToken
        public async Task<ApplicationUser?> FindUserOfRefreshTokenAndDeleteToken(string refreshToken)
        {
            RefreshToken? refreshTokenObject = await _refreshTokenRepository.GetAsync(refreshToken);

            if (refreshTokenObject is null || refreshTokenObject.ApplicationUser is null || refreshTokenObject.ExpirationTime < DateTime.UtcNow)
            {
                return null;
            }

            await _refreshTokenRepository.DeleteAsync(refreshTokenObject);

            return refreshTokenObject.ApplicationUser;
        }
        #endregion

        #region InvalidateUserTokensAsync
        public async Task InvalidateUserTokensAsync(Guid userId)
        {
            await _refreshTokenRepository.DeleteAllByUserIdAsync(userId);
        }
        #endregion
    }
}
