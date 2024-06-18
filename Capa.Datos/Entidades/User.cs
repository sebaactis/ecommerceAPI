using Microsoft.AspNetCore.Identity;

namespace Capa.Datos.Entidades
{
    public class User : IdentityUser
    {
        public ICollection<Orden> Ordenes { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
