

using Azure;
using Capa.Datos.Entidades;
using Capa.Datos.Modelos;
using Capa.Infraestructura;
using Capa.Infraestructura.Persistencia;
using Capa.Infraestructura.Servicios.Utilidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TestCarritoDeCompras
{
    public class AccountControllerShould
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenUtilities _tokenUtility;

        public AccountControllerShould()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddInMemoryCollection(new Dictionary<string, string>
                {
                    { "ConnectionStrings:DevConnection", "Data Source=DESKTOP-R0JOFDD; Initial Catalog=CarritoDeCompras;Trusted_Connection=True; TrustServerCertificate=True" }
                })
                .Build();

            var services = new ServiceCollection();

            services.AddRegistroInfra(configuration);

            services.AddLogging();

            var serviceProvider = services.BuildServiceProvider();

            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _tokenUtility = new TokenUtilities(configuration);
        }

        [Fact]
        public async void LoginOK()
        {

            var checkUser = await LoginProcess("sebaactis", "Carp1910@");

            Assert.Equal(checkUser, "Login OK");
            
        }

        [Fact]
        public async void LoginFailedForBadUser()
        {

            var checkUser = await LoginProcess("sebaacti", "Carp1910@");

            Assert.Equal(checkUser, "Login Not OK");

        }

        [Fact]
        public async void LoginFailedForBadPassword()
        {

            var checkUser = await LoginProcess("sebaactis", "Carp1910");

            Assert.Equal(checkUser, "Login Not OK");

        }

        public async Task<string> LoginProcess(string user, string password)
        {
            var userCheck = await _userManager.FindByNameAsync(user);

            if (user != null && await _userManager.CheckPasswordAsync(userCheck, password))
            {
                return "Login OK";
            }
            else
            {
                return "Login Not OK";
            }

        }
    }
}