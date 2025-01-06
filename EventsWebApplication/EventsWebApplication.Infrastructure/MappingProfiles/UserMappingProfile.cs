//Профили маппера разбиты на разные файлы

using EventsWebApplication.Core.Models;
using EventsWebApplication.Core.DTOs;
using System.Security.Cryptography;
using System.Text;

namespace EventsWebApplication.Infrastructure.MappingProfiles
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashPassword(src.Password)))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => GenerateSalt()))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => string.IsNullOrEmpty(src.Role) ? "User" : src.Role));

            CreateMap<User, UserRegisterDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());

            CreateMap<UserLoginDto, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => HashPassword(src.Password)))
                .ForMember(dest => dest.PasswordSalt, opt => opt.MapFrom(src => GenerateSalt()))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<User, UserLoginDto>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }

        private byte[] HashPassword(string password)
        {
            using var hmac = new HMACSHA512();
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private byte[] GenerateSalt()
        {
            using var rng = new RNGCryptoServiceProvider();
            var salt = new byte[16];
            rng.GetBytes(salt);
            return salt;
        }
    }
}
