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

    }
}
