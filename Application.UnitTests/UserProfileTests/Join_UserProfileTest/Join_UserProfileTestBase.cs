using Application.InternalServices.UserProfileServices;
using Domain.RepositoryInterfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.UserProfileTests.Join_UserProfileTest
{
    public class Join_UserProfileTestBase
    {
        private protected readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
        private protected readonly Mock<IJoinRepository> _joinRepositoryMock;

        private protected readonly Join_UserProfileService _join_UserProfileService;

        private protected Join_UserProfileTestBase()
        {
            _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            _joinRepositoryMock = new Mock<IJoinRepository>();
            _join_UserProfileService = new Join_UserProfileService(
                _announcementRepositoryMock.Object,
                _joinRepositoryMock.Object);
        }
    }
}
