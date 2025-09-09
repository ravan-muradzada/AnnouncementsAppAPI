using Application.ExternalServiceInterfaces;
using Domain.CustomExceptions;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices
{
    public class AccessTokenService : IAccessTokenService
    {
        #region Fields
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructor
        public AccessTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion

        #region SetClaims
        private Claim[] SetClaims(ApplicationUser user)
        {
            return new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim("security_stamp", user.SecurityStamp?? "_")
            };
        }
        #endregion

        #region CreateTokenGenerator
        private JwtSecurityToken CreateTokenGenerator(Claim[] claims)
        {
            DateTime expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:EXPIRATION_MINUTES"]));
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: expiration,
                    signingCredentials: credentials
            );
        }
        #endregion

        #region GenerateToken
        public string GenerateToken(ApplicationUser user)
        {
            Claim[] claims = SetClaims(user);
            JwtSecurityToken tokenGenerator = CreateTokenGenerator(claims);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string accessToken = tokenHandler.WriteToken(tokenGenerator);

            return accessToken;
        }
        #endregion

        #region ExtractDataFromJwtTokens
        public ClaimsPrincipal? ExtractDataFromJwtTokens(string? token)
        {
            if (token is null) throw new ObjectNotFoundException("Access Token Not Found!");
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "JUST_EXAMPLE_KEY_FOR_JWT_AUTHENTICATION_WHEN_THE_REAL_KEY_NOT_FOUND")),

                ValidateLifetime = false
            };

            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            ClaimsPrincipal principal = jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
        #endregion
    }
}
