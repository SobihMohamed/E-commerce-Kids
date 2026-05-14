using AutoMapper;
using E_commerce.Abstraction.IService.Profile;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.User;
using E_commerce.Shared.Dto_s.Profile;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.ProfileImplementaion
{
    public class ProfileService(UserManager<ApplicationUser> userManager, IMapper mapper) : IProfileService
    {
        public async Task<UserProfileDto> GetUserProfileAsync(string userId)
        {
            var user = await GetUserOrThrowAsync(userId);

            return mapper.Map<UserProfileDto>(user);
        }

        public async Task<UserProfileDto> UpdateProfileAsync(string userId, UpdateProfileDto dto)
        {
            var user = await GetUserOrThrowAsync(userId);
            // avoid auto-mapping for security reasons
            // (e.g., we don't want to allow updating the email or other sensitive fields through this method)
            user.FullName = dto.FullName;
            user.PhoneNumber = dto.PhoneNumber;

            // UserManager
            var result = await userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                throw new BadRequestExceptionCustome("Failed to update profile", errors);
            }

            return mapper.Map<UserProfileDto>(user);
        }

        // Helper Methods 
        private async Task<ApplicationUser> GetUserOrThrowAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null || user.IsDeleted)
                 throw new UserNotFoundException("User profile not found.");

            return user;
        }
    }
}
