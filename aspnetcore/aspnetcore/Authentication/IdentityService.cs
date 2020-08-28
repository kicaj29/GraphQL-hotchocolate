using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace aspnetcore.Authentication
{
    public interface IIdentityService
    {
        Task<string> Authenticate(string email, string password);
    }

    /// <summary>
    /// Normally this part is done on dedicated server like https://www.thinktecture.com/en/identityserver/
    /// </summary>
    public class IdentityService : IIdentityService
    {
        public async Task<string> Authenticate(string email, string password)
        {
            //Your custom logic here (e.g. database query)
            //Mocked for a sake of simplicity
            

            if ((email == "abc@dev.com") && (password == "pass123"))
            {
                var roles = new List<string>();
                roles.AddRange(new string[] { "Managers", "Senior Managers" });
                return GenerateAccessToken(email, Guid.NewGuid().ToString(), roles.ToArray());
            }

            throw new AuthenticationException();
        }

        private string GenerateAccessToken(string email, string userId, string[] roles)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("secretsecretsecret"));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, email)
            };

            claims = claims.Concat(roles.Select(role => new Claim(ClaimTypes.Role, role))).ToList();


            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "issuer",
                "audience",
                claims,
                expires: DateTime.Now.AddDays(90),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}