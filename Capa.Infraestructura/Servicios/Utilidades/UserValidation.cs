using Microsoft.AspNetCore.Http;

namespace Capa.Infraestructura.Servicios.Utilidades
{
    public static class UserValidation
    {
        public static string userValidationId(HttpContext httpContext)
        {
            var userId = httpContext.GetUserIdFromToken();

            if (userId == null)
            {
                return null;
            }

            return userId;
        }
    }
}
