using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
