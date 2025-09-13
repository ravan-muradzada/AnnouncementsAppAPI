using Application.ExternalServiceInterfaces;
using Application.InternalServiceInterfaces;
using Application.InternalServices;
using Application.Mappings;
using Domain.Entities;
using Domain.RepositoryInterfaces;
using Infrastructure.ExternalServices;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Infrastructure.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using starter_project_template.Filters.ExceptionFilters;
using System.Reflection.Metadata;
using System.Text;

namespace starter_project_template.StartupExtensions
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

            #region Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            #endregion

            #region DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
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
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<ITwoFactorService, TwoFactorService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
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
            services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddScoped<IRedisRepository, RedisRepository>();
            #endregion

            return services;
        }
    }
}
