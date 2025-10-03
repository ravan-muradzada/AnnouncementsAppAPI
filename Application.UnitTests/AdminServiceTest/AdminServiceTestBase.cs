using Application.ExternalServiceInterfaces;
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

namespace Application.UnitTests.AdminServiceTest
{
    public class AdminServiceTestBase
    {
        private protected readonly Mock<IAnnouncementRepository> _announcementRepositoryMock;
        private protected readonly Mock<IEmailService> _emailServiceMock;
        private protected readonly IMapper _mapper;

        private protected readonly AdminService _adminService;

        private protected AdminServiceTestBase()
        {
            _announcementRepositoryMock = new Mock<IAnnouncementRepository>();
            _emailServiceMock = new Mock<IEmailService>();
            var serviceProvider = new ServiceCollection()
            .AddLogging()
            .AddAutoMapper(
                _ => { },
                typeof(MappingProfile).Assembly)
            .BuildServiceProvider();
            _mapper = serviceProvider.GetRequiredService<IMapper>();

            _adminService = new AdminService(
                _announcementRepositoryMock.Object,
                _emailServiceMock.Object,
                _mapper);
        }
    }
}
