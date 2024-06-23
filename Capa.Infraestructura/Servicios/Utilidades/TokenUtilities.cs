
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Capa.Infraestructura.Servicios.Utilidades
{
    public class TokenUtilities
    {
        private readonly IConfiguration _configuration;
        public TokenUtilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateJwtToken(string username, string id, string userRole, int minutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim("Username", username),
                new Claim("UserId", id),
                new Claim(ClaimTypes.Role, userRole)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(minutes),
                signingCredentials: credentials
                );

            return tokenHandler.WriteToken(token);
        }


        public ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);
                var jwtToken = securityToken as JwtSecurityToken;

                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string RefreshToken(HttpContext context)
        {
            var refreshToken = context.Request.Cookies["RefreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return null;
            }

            var principal = ValidateToken(refreshToken);
            if (principal == null)
            {
                return null;
            }

            var username = principal.FindFirst("Username")?.Value;
            var userId = principal.FindFirst("UserId")?.Value;
            var userRole = principal.FindFirst(ClaimTypes.Role)?.Value;

            if (username == null || userId == null || userRole == null)
            {
                return null;
            }

            var newAccessToken = GenerateJwtToken(username, userId, userRole, 15);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };
            context.Response.Cookies.Append("accessToken", newAccessToken, cookieOptions);

            return newAccessToken;
        }
    }
}
