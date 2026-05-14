using AutoMapper;
using E_commerce.Domain.Models.User;
using E_commerce.Shared.Dto_s.Auth.Sign_In_Up;

namespace E_commerce.Services.AutoMapper.AuthMapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.DisplayName))
                .ForMember(dest => dest.CreatedAt , opt => opt.MapFrom(src => DateTime.UtcNow));
        }
    }
}