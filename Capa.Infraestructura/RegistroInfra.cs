using Capa.Aplicacion.Mapper;
using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Aplicacion.Servicios.Implementacion;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Persistencia;
using Capa.Infraestructura.Repositorio.Implementacion;
using Capa.Infraestructura.Repositorio.Interfaces;
using Capa.Infraestructura.Servicios.Implementacion;
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
                options.Cookie.Name = "JWT";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api") &&
                            context.Response.StatusCode == StatusCodes.Status200OK)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.WriteAsync("Usuario no autenticado").Wait();
                        }
                        else
                        {
                            context.Response.Redirect(context.RedirectUri);
                        }
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.WriteAsync("Usuario no autorizado").Wait();
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.Cookie.Name = "JWT";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.LoginPath = "/Account/Login";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
                options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        if (context.Request.Path.StartsWithSegments("/api") &&
                            context.Response.StatusCode == StatusCodes.Status200OK)
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            context.Response.WriteAsync("Usuario no autenticado").Wait();
                        }
                        else
                        {
                            context.Response.Redirect(context.RedirectUri);
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
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies["JWT"];
                            context.Token = token;
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdm", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireRole("Administrador");
                });

                options.AddPolicy("RequireMod", policy =>
                {
                    policy.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireRole("Moderador");
                });
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
