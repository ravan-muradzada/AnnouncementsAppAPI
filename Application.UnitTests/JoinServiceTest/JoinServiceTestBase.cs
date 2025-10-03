using Application.InternalServiceInterfaces;
using Application.InternalServices;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.JoinServiceTest
{
    public class JoinServiceTestBase
    {
        private protected readonly Mock<IJoinRepository> _joinRepositoryMock;
        private protected readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
        private protected readonly Mock<UserManager<ApplicationUser>> _userManagerMock;

        private protected readonly IJoinService _joinService;

        private protected JoinServiceTestBase()
        {
            _joinRepositoryMock = new Mock<IJoinRepository>();
            _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            _userManagerMock = MockUserManager();
            _joinService = new JoinService(
                _joinRepositoryMock.Object,
                _announcementRepositoryMock.Object,
                _userManagerMock.Object);
        }

        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        }
    }
}
