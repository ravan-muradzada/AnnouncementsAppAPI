using AnnouncemenetsAppAPI.Filters.ExceptionFilters;
using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Application.InternalServiceInterfaces.IUserProfileServices;
using Application.InternalServices;
using Application.InternalServices.UserProfileServices;
using Application.Mappings;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using FluentValidation;
using Infrastructure.ExternalServices;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Text;
using DotNetEnv;

namespace AnnouncemenetsAppAPI.StartupExtensions
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            #region Controllers
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiExceptionFilter>();
            });
            #endregion

            #region FluentValidation
            services.AddValidatorsFromAssembly(typeof(Program).Assembly, includeInternalTypes: true);
            #endregion

            #region Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            #endregion

            #region DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion

            #region Identity
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.User.RequireUniqueEmail = true; // might be changed
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<IUserValidator<ApplicationUser>, IdentityUsernameValidator>();
            #endregion

            #region JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? "JUST_EXAMPLE_KEY_FOR_JWT_AUTHENTICATION_WHEN_THE_REAL_KEY_NOT_FOUND")),
                        ClockSkew = TimeSpan.Zero
                    };
                });
            #endregion

            #region Authorization
            services.AddAuthorization();
            #endregion

            #region AutoMapper
            services.AddAutoMapper(_ => { }, typeof(MappingProfile).Assembly);
            #endregion

            #region Internal Services
            // User Profile Services
            services.AddScoped<IUserInfo_UserProfileService, UserInfo_UserProfileService>();
            services.AddScoped<ITwoFactorAuth_UserProfileService, TwoFactorAuth_UserProfileService>();
            services.AddScoped<IAnnouncement_UserProfileService, Announcement_UserProfileService>();

            // Other Services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ITwoFactorService, TwoFactorService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IJoinService, JoinService>();    
            #endregion

            #region External Services
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IAccessTokenService, AccessTokenService>();
            #endregion

            #region Redis
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = ConfigurationOptions.Parse("localhost:6379");
                configuration.AbortOnConnectFail = true;
                return ConnectionMultiplexer.Connect(configuration);
            });
            #endregion

            #region Repositories
            services.AddScoped<IRedisRepository, RedisRepository>();
            services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();
            services.AddScoped<IJoinRepository, JoinRepository>();
            #endregion

            return services;
        }
    }
}
