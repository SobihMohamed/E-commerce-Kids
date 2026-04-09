using E_commerce.Shared.Dto_s.Profile;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Profile
{
    public interface IProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(string userId);

        Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto);

    }
}
