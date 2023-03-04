using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using google2fa.API.Data;
using google2fa.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace google2fa.API.Services
{
    public interface ITokenService
    {
        string CreateToken(string id, int TokenTime);
    }
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["jwt:key"]));
        }
        public string CreateToken(string id, int TokenTime)
        {
            var claims = new List<Claim>{
                new Claim(JwtRegisteredClaimNames.NameId,id)
            };
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(TokenTime),
                SigningCredentials = creds,
                Issuer = _config["Jwt:Issuer"],
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


    }
}