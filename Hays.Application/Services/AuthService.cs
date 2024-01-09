using Hays.Application.Configuration;
using Hays.Application.Exceptions;
using Hays.Application.Services.Abstracts;
using Hays.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hays.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly AuthenticationConfiguration _authenticationConfiguration;
        private readonly IPasswordHasherService _passwordHasherService;

        public AuthService(AuthenticationConfiguration authenticationConfiguration,
            IPasswordHasherService passwordHasherService)
        {
            _authenticationConfiguration = authenticationConfiguration;
            _passwordHasherService = passwordHasherService;
        }

        public string GenerateTokenAsync(User user, string password)
        {
            if (!_passwordHasherService.ComparePasswordHash(password, user))
            {
                throw new BadRequestException("Invalid user login or password");
            }

            Claim[] claims = new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}")
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationConfiguration.JwtKey));
            SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            DateTime expires = DateTime.Now.Add(_authenticationConfiguration.JwtLifeTime);

            JwtSecurityToken token = new JwtSecurityToken(_authenticationConfiguration.JwtIssuer,
                _authenticationConfiguration.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: credentials);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
