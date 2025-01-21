using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Validators;
using EventsWebApplication.Infrastructure;
using EventsWebApplication.Infrastructure.Data;
using EventsWebApplication.Infrastructure.MappingProfiles;
using EventsWebApplication.Infrastructure.Repositories;
using EventsWebApplication.Infrastructure.Services;
using EventsWebApplication.Application.UseCases;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using EventsWebApplication.Core.Interfaces.Services;
using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Models;
using EventsWebApplication.Core.DTOs.ParticipantDTOs;
using EventsWebApplication.Application.UseCases.ParticipantUseCases;
using EventsWebApplication.Application;

namespace EventsWebApplication.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddUseCases();

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddAutoMapper(typeof(EventMappingProfile).Assembly);

            services.AddFluentValidation(fv =>
            {
                fv.RegisterValidatorsFromAssemblyContaining<EventValidator>();
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
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
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminPolicy", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "Admin"));
                options.AddPolicy("UserPolicy", policy =>
                    policy.RequireClaim(ClaimTypes.Role, "User", "Admin"));
            });

            return services;
        }

        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Введите токен в формате Bearer {токен}",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }
    }
}
