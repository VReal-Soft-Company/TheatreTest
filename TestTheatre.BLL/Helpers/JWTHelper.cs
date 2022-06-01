using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TestTheare.Shared.Data.Settings;
using TestTheatre.DAL.Entities;

namespace TestTheatre.BLL.Helpers
{
    public interface IJWTHelper
    {
        string GenerateJwtToken(User user, string loginProvider = "");
    }
    public class JWTHelper : IJWTHelper
    {
        private readonly JWTAuthentication _jwtAuthentication;
        public JWTHelper(JWTAuthentication jWTAuthenticationOptions)
        {
            _jwtAuthentication = jWTAuthenticationOptions;
        }
        public string GenerateJwtToken(User user, string loginProvider = "")
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtAuthentication.JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var hours = DateTime.Now.AddHours(Convert.ToDouble(_jwtAuthentication.JwtHours));
            var token = new JwtSecurityToken(
                _jwtAuthentication.JwtIssuer,
                _jwtAuthentication.JwtAudience,
                claims,
                expires: hours,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
