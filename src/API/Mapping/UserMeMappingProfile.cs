using AutoMapper;
using Autotest.Platform.API.DTOs.Users;
using Autotest.Platform.Domain.Entities;

namespace Autotest.Platform.API.Mapping
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