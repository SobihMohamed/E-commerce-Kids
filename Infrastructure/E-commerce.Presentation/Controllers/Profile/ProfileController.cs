using E_commerce.Abstraction.IService.Profile;
using E_commerce.Shared.Dto_s.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace E_commerce.Presentation.Controllers.Profile
{
    [Authorize]
    public class ProfileController(IProfileService profileService) : AppBaseController
    {
        [HttpGet]
        public async Task<ActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var profile = await profileService.GetUserProfileAsync(userId!);

            return Success(profile, "Profile retrieved successfully");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updatedProfile = await profileService.UpdateProfileAsync(userId!, dto);

            return Success(updatedProfile, "Profile updated successfully");
        }
    }
}