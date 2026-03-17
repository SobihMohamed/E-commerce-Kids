using E_commerce.Shared.Dto_s.Auth.ForgetPssword;
using E_commerce.Shared.Dto_s.Auth.Sign_In_Up;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Auth
{
    public interface IAuthService
    {
        Task<AuthModelDto> LoginAsync (LoginDto loginDto);
        Task<AuthModelDto> RegisterAsync (RegisterDto registerDto);
        Task ForgetPasswordAsync (ForgetPasswordDto forgetPasswordDto); // return otp to reset password
        Task<bool> VerifyOtpAsync(VerifyOtpDto verifyOtpDto);
        Task<AuthModelDto> ResetPasswordAsync (ResetPasswordDto resetPasswordDto); // to still login after reset password
    }
}
