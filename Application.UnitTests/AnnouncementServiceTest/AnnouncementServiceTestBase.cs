using Application.InternalServiceInterfaces;
using Application.InternalServices;
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

namespace Application.UnitTests.AnnouncementServiceTest
{
    public class AnnouncementServiceTestBase
    {
        private protected readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
        private protected readonly IMapper _mapper;
        
        private protected readonly IAnnouncementService _announcementService;

        private protected AnnouncementServiceTestBase()
        {
            _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddAutoMapper(
                _ => { },
                typeof(MappingProfile).Assembly)
            .BuildServiceProvider();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _announcementService = new AnnouncementService(_announcementRepositoryMock.Object, _mapper);
        }
    }
}
