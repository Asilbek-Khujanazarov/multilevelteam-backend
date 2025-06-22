using AutoMapper;
using Autotest.Platform.API.DTOs.Users;
using Autotest.Platform.Domain.Entities;
using Autotest.Platform.Domain.Interfaces;

namespace Autotest.Platform.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserMeDto> GetCurrentUserAsync(Guid userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return _mapper.Map<UserMeDto>(user);
        }

        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByTelegramChatIdAsync(string chatId)
        {
            var user = await _userRepository.GetByTelegramChatIdAsync(chatId);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<TelegramUserDto> GetTelegramUserByPhoneNumberAsync(string phoneNumber)
        {
            var telegramUser = await _userRepository.GetTelegramUserByPhoneNumberAsync(phoneNumber);
            return _mapper.Map<TelegramUserDto>(telegramUser);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteAsync(id);
        }

      
    }
}