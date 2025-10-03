using Application.InternalServiceInterfaces;
using Application.InternalServices;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.RefreshTokenServiceTest
{
    public class RefreshTokenServiceTestBase
    {
        private protected IConfiguration _configuration;
        private protected Mock<IRedisRepository> _redisRepositoryMock;
        private protected Mock<UserManager<ApplicationUser>> _userManagerMock;

        private protected IRefreshTokenService _refreshTokenService;

        private protected RefreshTokenServiceTestBase()
        {
            var inMemorySettings = new Dictionary<string, string?>
            {
                { "RefreshToken:EXPIRATION_MINUTES", "10" }
            };
            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
            _redisRepositoryMock = new Mock<IRedisRepository>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            _refreshTokenService = new RefreshTokenService(_configuration, _redisRepositoryMock.Object, _userManagerMock.Object);
        }
    }
}
