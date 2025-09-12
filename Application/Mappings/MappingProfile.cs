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
        }

        private void ConfigureUserProfileMappings()
        {
            CreateMap<ApplicationUser, UserProfileResponse>();
        }
    }
}
