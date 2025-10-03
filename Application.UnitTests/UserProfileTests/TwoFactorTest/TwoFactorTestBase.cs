using Application.InternalServices.UserProfileServices;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.UserProfileTests.TwoFactorTest
{
    public class TwoFactorTestBase
    {
        private protected readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        private protected readonly TwoFactorAuth_UserProfileService _twoFactorAuthService;

        private protected TwoFactorTestBase()         {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _twoFactorAuthService = new TwoFactorAuth_UserProfileService(_userManagerMock.Object);
        }
    }
}
