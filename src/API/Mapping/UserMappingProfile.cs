using AutoMapper;
using Multilevelteam.Platform.API.DTOs.Users;
using Multilevelteam.Platform.Domain.Entities;

namespace Multilevelteam.Platform.API.Mapping
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