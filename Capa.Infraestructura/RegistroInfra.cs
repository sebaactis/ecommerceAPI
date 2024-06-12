﻿using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Aplicacion.Servicios.Implementacion;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Infraestructura.Persistencia;
using Capa.Infraestructura.Repositorio.Implementacion;
using Capa.Infraestructura.Repositorio.Interfaces;
using Capa.Infraestructura.Servicios.Implementacion;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Capa.Infraestructura
{
    public static class RegistroInfra
    {
        public static IServiceCollection AddRegistroInfra(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing";

            if (useInMemory)
            {
                services.AddDbContext<CarritoDbContext>(options =>
                options.UseInMemoryDatabase("MemoryDatabase"));
            }
            else
            {
                services.AddDbContext<CarritoDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DevConnection"), b => b.MigrationsAssembly("CarritoDeCompras")));
            }

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<CarritoDbContext>()
                .AddDefaultTokenProviders();



            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddScoped<ICartRepositorio, CartRepositorio>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICartService, CartService>();

            return services;
        }

    }
}
