using Application.InternalServiceInterfaces;
using Domain.CustomExceptions;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
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
        private readonly IConfiguration _configuration;
        private readonly IRedisRepository _redisRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Constructor
        public RefreshTokenService(IConfiguration configuration, IRedisRepository redisRepository, UserManager<ApplicationUser> userManager)
        {
            _configuration = configuration;
            _redisRepository = redisRepository;
            _userManager = userManager;
        }
        #endregion

        #region GenerateRefreshToken
        public async Task<string> GenerateRefreshToken(Guid userId)
        {
            byte[] bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(bytes);
            string refreshToken = Convert.ToBase64String(bytes);
            var expirationTime = Convert.ToDouble(_configuration["RefreshToken:EXPIRATION_MINUTES"] ?? "10");

            string keyForRedis = $"Refresh_Token:{userId}:{refreshToken}";
            await _redisRepository.SetStringAsync(keyForRedis, "valid", TimeSpan.FromMinutes(expirationTime));
            return refreshToken;
        }
        #endregion

        #region FindUserOfRefreshTokenAndDeleteToken
        public async Task<ApplicationUser?> FindUserOfRefreshTokenAndDeleteToken(string refreshToken)
        {
            string pattern = $"Refresh_Token:*:{refreshToken}";
            IEnumerable<string> result = _redisRepository.GetByPattern(pattern);
            
            if (result.Count() == 0)
            {
                throw new ObjectNotFoundException("Refresh Token Not Found!");
            }

            string key = result.First();
            string[] parts = key.Split(':');
            if (parts.Length != 3)
            {
                throw new InvalidCredentialsException("Refresh Token key is invalid!");
            }

            Guid userId = Guid.Parse(parts[1]);

            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString()) ?? throw new ObjectNotFoundException("User Not Found");

            await _redisRepository.DeleteAsync(key);

            return user;
        }
        #endregion

        #region InvalidateUserTokensAsync
        public async Task InvalidateUserTokensAsync(Guid userId)
        {
            await _redisRepository.DeleteByPattern($"Refresh_Token:{userId}:*");
        }
        #endregion
    }
}
