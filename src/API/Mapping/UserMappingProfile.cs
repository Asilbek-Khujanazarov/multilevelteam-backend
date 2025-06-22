using AutoMapper;
using Autotest.Platform.API.DTOs.Users;
using Autotest.Platform.Domain.Entities;

namespace Autotest.Platform.API.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<TelegramUser, TelegramUserDto>();
        }
    }
}