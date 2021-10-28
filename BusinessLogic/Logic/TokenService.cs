using Core.Entities;
using Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Logic
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey securityKey;
        private readonly IConfiguration config;

        public TokenService(IConfiguration config)
        {
            this.config = config;
            this.securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
        }

        public string CreateToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Name, usuario.Nombre),
                new Claim(JwtRegisteredClaimNames.FamilyName, usuario.Apellido),
                new Claim("username", usuario.UserName)
            };

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenConfiguration = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(60),
                SigningCredentials = credentials,
                Issuer = config["Token:Issuer"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenConfiguration);

            return tokenHandler.WriteToken(token);
        }
    }
}
