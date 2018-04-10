using System.Threading.Tasks;
using JWTExample.Dto.Account;
using Microsoft.AspNetCore.Identity;

namespace JWTExample.BusinessLogic.Interfaces
{
    public interface IAuthService
    {
        Task<TokenDto> Login(LoginUserDto loginUserDto);
        Task<IdentityResult> Register(RegisterUserDto registerUserDto);
    }
}
