using AutoMapper;
using E_commerce.Abstraction.IService.Auth;
using E_commerce.Abstraction.IService.Token;
using E_commerce.Domain.Exceptions;
using E_commerce.Domain.Exceptions.NotFoundModels;
using E_commerce.Domain.Models.User;
using E_commerce.Shared.Dto_s.Auth.Sign_In_Up;
using E_commerce.Shared.Dto_s.Token;
using E_commerce.Shared.EnumsHelper.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Services.Services.AuthImplementation
{
    public class AuthService(UserManager<ApplicationUser> _userManager , IMapper _mapper , ITokenService _tokenService) : IAuthService
    {
        public async Task<AuthModelDto> RegisterAsync(RegisterDto registerDto)
        {
            // 1 - Check if the user already exists
            var user = await _userManager.FindByEmailAsync(registerDto.Email);
            if (user == null) 
                throw new BadRequestExceptionCustome("User with this email already exists.");

            // 2 - mapping the data from the DTO to the ApplicationUser model
            var userForDB = _mapper.Map<ApplicationUser>(registerDto);

            // 3 - Create the user in the database
            var result = await _userManager.CreateAsync(userForDB, registerDto.Password);

            // 4 - Check if the user creation was successful
            if (!result.Succeeded)
            {
                // If user creation failed, throw an exception with the error details
                var errors = result.Errors.Select(e => e.Description);
                throw new BadRequestExceptionCustome("User registration failed" , errors);
            }
            
            // add the user to the "Customer" role
            await _userManager.AddToRoleAsync(userForDB, UserType.Customer.ToString()); 
            
            var userRoles = await _userManager.GetRolesAsync(userForDB);
            // 5 - Generate Token
            var tokenRequest = new TokenRequestDto
            {
                UserId = userForDB.Id,
                Email = userForDB.Email,
                UserName = userForDB.FullName,
                Roles = userRoles
            };
            var tokenResponse = await _tokenService.CreateTokenAsync(tokenRequest);
            if (string.IsNullOrEmpty(tokenResponse.Token))
                throw new BadRequestExceptionCustome("Token generation failed.");

            // 6 - Return the authentication model with the token and user details
            return new AuthModelDto
            {
                Token = tokenResponse.Token,
                IsAuthenticated = true,
                ExpireOn = tokenResponse.ExpireOn,
                Email = userForDB.Email!,
                Name = userForDB.FullName,
                Roles = userRoles
            };


        }
        public async Task<AuthModelDto> LoginAsync(LoginDto loginDto)
        {
         
            // 1 - Get the user from the database
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            // 2 - Check if the user exists
            if (user == null)
                throw new UnauthorizedExceptionCusotme();

            // 3 - Check if the password is correct
            var result = await _userManager.CheckPasswordAsync(user!, loginDto.Password);
            if (!result)
                throw new UnauthorizedExceptionCusotme();

            // 4 - Generate Token
            var userRoles = await _userManager.GetRolesAsync(user!);
            var tokenRequest = new TokenRequestDto
            {
                UserId = user!.Id!,
                Email = user.Email!,
                UserName = user.FullName,
                Roles = userRoles
            };
            var tokenResp = await _tokenService.CreateTokenAsync(tokenRequest);
            if (string.IsNullOrEmpty(tokenResp.Token))
                throw new BadRequestExceptionCustome("Token generation failed.");

            // 5 - Return the authentication model with the token and user details
            return new AuthModelDto
            {
                Token = tokenResp.Token,
                IsAuthenticated = true,
                ExpireOn = tokenResp.ExpireOn,
                Email = user.Email!,
                Name = user.FullName,
                Roles = userRoles
            };
        }
    }
}
