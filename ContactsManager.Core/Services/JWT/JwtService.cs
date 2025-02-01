using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.DTOs.AuthenticationJWT;
using ContactsManager.Core.ServiceContracts.JWTContract;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ContactsManager.Core.Services.JWT
{
    public class JwtService : IJWTService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthenticationResponse CreateJwtToken(ApplicationUser user)
        {
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JWT:EXPIRATION_MINUTES"]));
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub , user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti ,  Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat , DateTime.UtcNow.ToString()),
                new Claim(ClaimTypes.NameIdentifier , user.Email.ToString()),
                new Claim(ClaimTypes.Name , user.PersonName.ToString()),
            };

            var secretKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _configuration["JWT:Key"]));
            var SigningCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenGenerator = new JwtSecurityToken(
                _configuration["JWT:Issure"],
                _configuration["JWT:Audience"],
                claims,
                expires: expiration,
                signingCredentials: SigningCredentials
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken( tokenGenerator );
            return new AuthenticationResponse()
            {
                Token = token,
                Email = user.Email,
                PersonName = user.PersonName,
                Expiration = expiration,
                RefreshToken = GenerateRefreshToken(),
                RefreshTokenExpirationDateTime = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["RefreshToken:EXPIRATION_MINUTES"])),
            };
        }
        // create a refresh token (base 64 string of randum number)
        private string GenerateRefreshToken()
        {
            var bytes = new byte[64];
            var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes( bytes );
            return Convert.ToBase64String(bytes);

        }
    }
}
