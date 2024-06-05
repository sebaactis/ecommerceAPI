using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Datos.Entidades
{
    public class Cart : ModelBase
    {
        public int CartId { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
