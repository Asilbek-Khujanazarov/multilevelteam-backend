using AutoMapper;
using Autotest.Platform.API.DTOs.Users;
using Autotest.Platform.Domain.Entities;

namespace Autotest.Platform.API.Mapping
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()));

            CreateMap<TelegramUser, TelegramUserDto>();
        }
    }
}