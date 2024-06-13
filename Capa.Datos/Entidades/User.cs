using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Datos.Entidades
{
    public class User : IdentityUser
    {
        public ICollection<Orden> Ordenes { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }
}
