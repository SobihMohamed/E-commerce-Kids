using E_commerce.Shared.Dto_s.Token;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_commerce.Abstraction.IService.Token
{
    public interface ITokenService
    {
        Task<TokenResponseDto> CreateTokenAsync(TokenRequestDto tokenRequest);

    }
}
