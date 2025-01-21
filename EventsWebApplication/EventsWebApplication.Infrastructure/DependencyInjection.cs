using Microsoft.Extensions.DependencyInjection;
using EventsWebApplication.Core.Interfaces.Repositories;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Infrastructure.Repositories;
using EventsWebApplication.Infrastructure;
using EventsWebApplication.Core.Interfaces.Services;
using EventsWebApplication.Infrastructure.Services;
using EventsWebApplication.Infrastructure.MappingProfiles;
using EventsWebApplication.Infrastructure.Data;
using Microsoft.Extensions.Configuration;

namespace EventsWebApplication.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastucture(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EventAppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IPasswordService, PasswordService>();
            services.AddScoped<ITokenService, TokenService>();

            services.AddAutoMapper(typeof(EventMappingProfile).Assembly);

            return services;
        }
    }
}
