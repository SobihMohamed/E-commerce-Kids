using E_commerce.Abstraction.IService.Auth;
using E_commerce.Shared.Common.Responses; 
using E_commerce.Shared.Dto_s.Auth.ForgetPssword;
using E_commerce.Shared.Dto_s.Auth.Sign_In_Up;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Presentation.Controllers.Auth
{
    public class AuthController : AppBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // 1. (Register)
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<AuthModelDto>), 200)]
        public async Task<ActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Success(result, "Account Created Successfully");
        }

        // 2. (Login)
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<AuthModelDto>), 200)]
        public async Task<ActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Success(result, "Login Successfully");
        }

        // 3. (Forget Password)
        [HttpPost("forget-password")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)] 
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto)
        {
            await _authService.ForgetPasswordAsync(dto);
            return Success("Send OTP to your email");
        }

        // 4. Check OTP
        [HttpPost("verify-otp")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)] 
        [ProducesResponseType(typeof(ApiResponse<object>), 400)] 
        public async Task<ActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var isValid = await _authService.VerifyOtpAsync(dto);

            if (!isValid)
                return BadRequestError("OTP Not Valid or Incorrect");

            return Success("OTP is Valid");
        }

        // 5. (Reset Password)
        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse<AuthModelDto>), 200)]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            return Success(result, "your password has changed successfully");
        }
    }
}