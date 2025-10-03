using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Application.InternalServices;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.TwoFactorTest
{
    public class TwoFactorTestBase
    {
        private protected readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private protected readonly Mock<IEmailService> _emailServiceMock;

        private protected readonly ITwoFactorService _twoFactorService;

        private protected TwoFactorTestBase()
        {
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            _emailServiceMock = new Mock<IEmailService>();

            _twoFactorService = new TwoFactorService(_userManagerMock.Object, _emailServiceMock.Object);
        }
    }
}
