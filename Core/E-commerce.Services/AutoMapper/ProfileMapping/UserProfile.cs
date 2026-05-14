using AutoMapper;
using E_commerce.Domain.Models.User;
using E_commerce.Shared.Dto_s.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.AutoMapper.ProfileMapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<ApplicationUser, UserProfileDto>();
        }
    }
}
