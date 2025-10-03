using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Application.InternalServiceInterfaces.IUserProfileServices;
using Application.InternalServices.UserProfileServices;
using Application.Mappings;
using AutoMapper;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.UserProfileTests.UserInfoTest
{
    public class UserInfoTestBase
    {
        private protected readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private protected readonly IMapper _mapper;
        private protected readonly Mock<IEmailService> _emailServiceMock;
        private protected readonly Mock<IRedisRepository> _redisRepositoryMock;
        private protected readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;

        private protected readonly IUserInfo_UserProfileService _userInfoService;

        private protected UserInfoTestBase()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddAutoMapper(
                _ => { },
                typeof(MappingProfile).Assembly)
            .BuildServiceProvider();
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _emailServiceMock = new Mock<IEmailService>();
            _redisRepositoryMock = new Mock<IRedisRepository>();
            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            _userInfoService = new UserInfo_UserProfileService(
                _userManagerMock.Object,
                _mapper,
                _emailServiceMock.Object,
                _redisRepositoryMock.Object,
                _refreshTokenServiceMock.Object
            );
        }
    }
}
