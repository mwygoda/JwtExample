using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JWTExample.BusinessLogic.Interfaces;
using JWTExample.DataAccess.Models;
using JWTExample.Dto.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTExample.BusinessLogic.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly PasswordHasher<User> _passwordHasher;
        private readonly IConfigurationRoot _configuration;
        private readonly IMapper _mapper;

        public AuthService(UserManager<User> userManager, PasswordHasher<User> passwordHasher, IConfigurationRoot configuration, IMapper mapper)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<TokenDto> Login(LoginUserDto loginUserDto)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == loginUserDto.Email);
            var token = ConfirmUserPersonality(user, loginUserDto) ? GetJwtSecurityToken(user) : null;
            if (user == null || token == null)
            {
                return null;
            }
            return new TokenDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public async Task<IdentityResult> Register(RegisterUserDto registerUserDto)
        {
            var user = _mapper.Map<RegisterUserDto, User>(registerUserDto);
            var result = await _userManager.CreateAsync(user, registerUserDto.Password);

            return result;
        }

        private bool ConfirmUserPersonality(User user, LoginUserDto loginUserDto)
        {
            if (user == null)
            {
                return false;
            }
            var resultOfveryfyingPassword =
                _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUserDto.Password);
            return resultOfveryfyingPassword == PasswordVerificationResult.Success;
        }

        private JwtSecurityToken GetJwtSecurityToken(User user)
        {
            return new JwtSecurityToken(
                issuer: _configuration["TokenConfiguration:Issuer"],
                audience: _configuration["TokenConfiguration:Audience"],
                claims: GetTokenClaims(user),
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["TokenConfiguration:TimeInMinutesOfJwtLife"])),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenConfiguration:Key"])), SecurityAlgorithms.HmacSha256)
            );
        }

        private IEnumerable<Claim> GetTokenClaims(User user)
        {
            return new List<Claim>
         {
             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
             new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
             new Claim(JwtRegisteredClaimNames.Email, user.Email),
             new Claim(JwtRegisteredClaimNames.NameId, user.Id)
         };
        }
    }
}
