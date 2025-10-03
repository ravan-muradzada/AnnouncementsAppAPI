using Application.InternalServiceInterfaces.IUserProfileServices;
using Application.InternalServices.UserProfileServices;
using Application.Mappings;
using AutoMapper;
using Domain.RepositoryInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UnitTests.UserProfileTests.Announcement_UserProfileTest
{
    public class Announcement_UserProfileTestBase
    {
        private protected readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
        private protected readonly IMapper _mapper;

        private protected readonly IAnnouncement_UserProfileService _userProfileServiceMock;

        private protected Announcement_UserProfileTestBase()
        {
            _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddAutoMapper(
                _ => { },
                typeof(MappingProfile).Assembly)
            .BuildServiceProvider();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _userProfileServiceMock = new Announcement_UserProfileService(_announcementRepositoryMock.Object, _mapper);
        }
    }
}
