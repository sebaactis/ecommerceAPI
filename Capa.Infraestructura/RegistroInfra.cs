using Capa.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Infraestructura
{
    public static class RegistroInfra
    {
        public static IServiceCollection AddRegistroInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CarritoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("CarritoDeCompras")));

            return services;
        }

    }
}
