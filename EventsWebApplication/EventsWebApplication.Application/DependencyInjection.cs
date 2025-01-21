using EventsWebApplication.Application.UseCases;
using EventsWebApplication.Application.UseCases.EventUseCases;
using EventsWebApplication.Application.UseCases.ParticipantUseCases;
using EventsWebApplication.Core.DTOs.ParticipantDTOs;
using EventsWebApplication.Core.DTOs.UserDTOs;
using EventsWebApplication.Core.DTOs;
using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace EventsWebApplication.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            services.AddScoped<GetAllEventsUseCase>();
            services.AddScoped<GetEventByIdUseCase>();
            services.AddScoped<GetEventByNameUseCase>();
            services.AddScoped<AddEventUseCase>();
            services.AddScoped<UpdateEventUseCase>();
            services.AddScoped<DeleteEventUseCase>();
            services.AddScoped<UploadEventImageUseCase>();
            services.AddScoped<GetEventImageUseCase>();
            services.AddScoped<GetPaginatedEventsUseCase>();
            services.AddScoped<GetEventsByParticipantWithFilterUseCase>();
            services.AddScoped<GetAvailableSeatsUseCase>();

            services.AddScoped<IUseCase<UserRegisterDto, string>, RegisterUserUseCase>();
            services.AddScoped<IUseCase<UserLoginDto, object>, LoginUserUseCase>();
            services.AddScoped<IUseCase<RefreshTokenRequestDto, object>, RefreshTokenUseCase>();

            services.AddScoped<IUseCase<RegisterParticipantRequestDto, Unit>, RegisterParticipantUseCase>();
            services.AddScoped<IUseCase<UpdateParticipantRequestDto, Unit>, UpdateParticipantUseCase>();
            services.AddScoped<IUseCase<DeleteParticipantFromEventRequestDto, Unit>, DeleteParticipantFromEventUseCase>();
            services.AddScoped<IUseCase<GetParticipantsByEventIdRequestDto, IEnumerable<ParticipantResponseDto>>, GetParticipantsByEventIdUseCase>();
            services.AddScoped<IUseCase<GetParticipantByIdRequestDto, ParticipantResponseDto>, GetParticipantByIdUseCase>();

            services.AddScoped<IUseCase<NotificationRequestDto, IEnumerable<Notification>>, NotificationUseCase>();

            return services;
        }
    }
}
