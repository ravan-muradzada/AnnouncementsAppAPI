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
            // (Guid Id, string Title, string Content, string Category, bool IsExpired, DateTime? ExpiresAt, DateTime? PublishedAt, bool IsPublished, bool IsPinned)
            CreateMap<CreateAnnouncementRequest, Announcement>();
            CreateMap<Announcement, AnnouncementResponse>()
                .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Title", opt => opt.MapFrom(src => src.Title))
                .ForCtorParam("Content", opt => opt.MapFrom(src => src.Content))
                .ForCtorParam("Category", opt => opt.MapFrom(src => src.Category))
                .ForCtorParam("ExpiresAt", opt => opt.MapFrom(src => src.ExpiresAt))
                .ForCtorParam("PublishedAt", opt => opt.MapFrom(src => src.PublishedAt))
                .ForCtorParam("IsPublished", opt => opt.MapFrom(src => src.IsPublished))
                .ForCtorParam("IsPinned", opt => opt.MapFrom(src => src.IsPinned))
                .ForCtorParam("IsExpired", opt => opt.MapFrom(src => src.ExpiresAt < DateTime.UtcNow))
                .ForMember(
                    dest => dest.IsExpired,
                    opt => opt.MapFrom(src => src.ExpiresAt < DateTime.UtcNow)
                );
            CreateMap<UpdateAnnouncementRequest, Announcement>();
        }
    }
}
