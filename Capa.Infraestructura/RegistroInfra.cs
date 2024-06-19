using Capa.Aplicacion.Mapper;
using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Aplicacion.Servicios.Implementacion;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Persistencia;
using Capa.Infraestructura.Repositorio.Implementacion;
using Capa.Infraestructura.Repositorio.Interfaces;
using Capa.Infraestructura.Servicios.Implementacion;
using Capa.Infraestructura.Servicios.Utilidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;



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

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<CarritoDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        if (ctx.Request.Path.StartsWithSegments("/api") &&
                            ctx.Response.StatusCode == StatusCodes.Status200OK)
                        {
                            ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            ctx.Response.WriteAsync("Usuario no autenticado").Wait();
                        }
                        else
                        {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.CompletedTask;
                    }
                };
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };
                });


            services.AddScoped(typeof(IRepositorioBase<>), typeof(RepositorioBase<>));
            services.AddScoped<ICartRepositorio, CartRepositorio>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IOrdenRepositorio, OrdenRepositorio>();
            services.AddScoped<IOrdenService, OrdenService>();

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }

    }
}
