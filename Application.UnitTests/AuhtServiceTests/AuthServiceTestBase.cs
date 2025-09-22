using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Application.InternalServices;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Application.UnitTests.AuhtServiceTests
{
    public class AuthServiceTestBase
    {
        private protected readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private protected readonly Mock<IEmailService> _emailServiceMock;
        private protected readonly Mock<IAccessTokenService> _accessTokenServiceMock;
        private protected readonly Mock<IRefreshTokenService> _refreshTokenServiceMock;
        private protected readonly Mock<ITwoFactorService> _twoFactorServiceMock;
        private protected readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private protected readonly Mock<IRedisRepository> _redisRepositoryMock;

        private protected readonly IAuthService _authService;
        private protected AuthServiceTestBase()
        {
            _userManagerMock = MockUserManager();
            _emailServiceMock = new Mock<IEmailService>();
            _accessTokenServiceMock = new Mock<IAccessTokenService>();
            _refreshTokenServiceMock = new Mock<IRefreshTokenService>();
            _twoFactorServiceMock = new Mock<ITwoFactorService>();
            _redisRepositoryMock = new Mock<IRedisRepository>();

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "https";
            httpContext.Request.Host = new HostString("localhost", 5001);

            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            _authService = new AuthService(
                _userManagerMock.Object,
                _emailServiceMock.Object,
                _accessTokenServiceMock.Object,
                _refreshTokenServiceMock.Object,
                _twoFactorServiceMock.Object,
                _httpContextAccessorMock.Object,
                _redisRepositoryMock.Object
            );
        }
        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }
    }

}
