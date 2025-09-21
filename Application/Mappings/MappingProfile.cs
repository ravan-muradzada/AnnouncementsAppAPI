using Application.DTOs.Announcement.Request;
using Application.DTOs.Announcement.Response;
using Application.DTOs.UserProfile.Response;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() { 
            ConfigureUserProfileMappings();
            ConfigureAnnouncementMappings();
        }

        private void ConfigureUserProfileMappings()
        {
            CreateMap<ApplicationUser, UserProfileResponse>();
        }

        private void ConfigureAnnouncementMappings()
        {
            CreateMap<CreateAnnouncementRequest, Announcement>();
            CreateMap<Announcement, AnnouncementResponse>();
            CreateMap<UpdateAnnouncementRequest, Announcement>();
        }
    }
}
