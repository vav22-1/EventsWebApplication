﻿//Профили маппера разбиты на разные файлы

using EventsWebApplication.Core.Models;
using EventsWebApplication.Application.DTOs.UserDTOs;
using EventsWebApplication.Infrastructure.Services;

namespace EventsWebApplication.Infrastructure.MappingProfiles
{
    public class UserLoginMappingProfile : Profile
    {
        public UserLoginMappingProfile()
        {
            CreateMap<UserLoginDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var passwordService = (PasswordService)context.Items["PasswordService"];
                    return passwordService.HashPassword(src.Password, (byte[])context.Items["Salt"]);
                }))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom((src, dest, destMember, context) =>
                {
                    var passwordService = (PasswordService)context.Items["PasswordService"];
                    return passwordService.GenerateSalt();
                }))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<User, UserLoginDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}
