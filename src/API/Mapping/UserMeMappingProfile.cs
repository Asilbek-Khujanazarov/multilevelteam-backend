using AutoMapper;
using Multilevelteam.Platform.API.DTOs.Users;
using Multilevelteam.Platform.Domain.Entities;

namespace Multilevelteam.Platform.API.Mapping
{
    public class UserMeMappingProfile : Profile
    {
        public UserMeMappingProfile()
        {
            CreateMap<User, UserMeDto>();

            CreateMap<TelegramUser, TelegramUserMeDto>();
        }
    }
}