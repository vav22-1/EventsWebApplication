using EventsWebApplication.Core.Interfaces;
using EventsWebApplication.Core.DTOs.UserDTOs;
using EventsWebApplication.Core.Interfaces.Services;
using EventsWebApplication.Core.Models;
using System;
using System.Threading.Tasks;

namespace EventsWebApplication.Application.UseCases
{
    public class RegisterUserUseCase : IUseCase<UserRegisterDto, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;

        public RegisterUserUseCase(IUnitOfWork unitOfWork, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
        }

        public async Task<string> ExecuteAsync(UserRegisterDto request)
        {
            if (_unitOfWork.Users.UserExists(request.Username))
            {
                throw new InvalidOperationException("Пользователь с таким именем уже существует");
            }

            var salt = _passwordService.GenerateSalt();
            var hashedPassword = _passwordService.HashPassword(request.Password, salt);

            var newParticipant = new Participant
            {
                DateOfRegistration = DateTime.UtcNow
            };

            await _unitOfWork.Participants.AddAsync(newParticipant);

            var user = new User
            {
                Username = request.Username,
                Role = request.Role,
                Password = hashedPassword,
                PasswordSalt = salt,
                Participant = newParticipant
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return "Успешная регистрация";
        }
    }
}
