using System.Threading.Tasks;
using JWTExample.BusinessLogic.Interfaces;
using JWTExample.Dto.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JWTExample.API.Controllers
{
    [Route("/api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromBody] LoginUserDto loginUserDto)
        {
            var tokenResponse = await _authService.Login(loginUserDto);

            if (tokenResponse?.Token != null) return Ok(tokenResponse.Token);
            if (tokenResponse == null)
                return BadRequest();
            return Ok(tokenResponse.Token);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            var registerResult = await _authService.Register(registerUserDto);

            if (registerResult.Succeeded)
                return NoContent();
            return BadRequest();
        }

        [HttpGet]
        [Route("TopSecret")]
        [Authorize]
        public async Task<IActionResult> TopSecretAction()
        {
            return Ok("You will get top secret data");
        }
    }
}
