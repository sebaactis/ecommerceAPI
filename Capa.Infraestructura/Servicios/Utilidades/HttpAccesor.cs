using Microsoft.AspNetCore.Http;

namespace Capa.Infraestructura.Servicios.Utilidades
{
    public class HttpAccesor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public HttpAccesor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string getUserIdToken()
        {
            var httpContext = _contextAccessor.HttpContext;

            if (httpContext != null)
            {
                var userId = httpContext.User.FindFirst("UserId")?.Value;
                return userId;
            }

            return null;
        }
    }
}
