using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Capa.Infraestructura.Servicios.Utilidades
{
        public static class HttpContextExtensions
        {
            public static string GetJwtToken(this HttpContext context)
            {
                var jwtFromCookie  = context.Request.Cookies.TryGetValue("accessToken", out string token);

                if(!jwtFromCookie) return null;

                return token;
            }

            public static string GetUserIdFromToken(this HttpContext context)
            {
                 var token = context.GetJwtToken();
                if (string.IsNullOrEmpty(token))
                {
                    return null;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserId");
                return userIdClaim?.Value;
            }
        }
    }
