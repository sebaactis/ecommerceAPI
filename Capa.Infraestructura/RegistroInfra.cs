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

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
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
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var tokenUtility = new TokenUtilities(configuration);
                            var accessToken = context.Request.Cookies["accessToken"];

                            if (context.Request.Path == "/api/Account/Login")
                            {
                                return Task.CompletedTask;
                            }

                            if (!string.IsNullOrEmpty(accessToken) && context.Request.Path != "/api/Account/Login")
                            {
                                var principal = tokenUtility.ValidateToken(accessToken);

                                if (principal != null)
                                {
                                    context.Token = accessToken;
                                    return Task.CompletedTask;
                                }
                            }

                            var refreshToken = context.Request.Cookies["refreshToken"];

                            if (!string.IsNullOrEmpty(refreshToken) && context.Request.Path != "/api/Account/Login")
                            {
                                var newAccessToken = tokenUtility.RefreshToken(context.HttpContext);

                                if (newAccessToken != null)
                                {
                                    context.Token = newAccessToken;
                                    return Task.CompletedTask;
                                }
                            }

                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"No autorizado. Debe autenticarse para acceder a este recurso.\"}");
                        },
                        OnAuthenticationFailed = context =>
                        {
                            context.NoResult();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "text/plain";
                            return context.Response.WriteAsync("Token no válido.");
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"No autorizado. Debe autenticarse para acceder a este recurso.\"}");
                        },
                        OnForbidden = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
                            context.Response.ContentType = "application/json";
                            return context.Response.WriteAsync("{\"error\": \"Prohibido. No tiene permiso para acceder a este recurso.\"}");
                        }
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
