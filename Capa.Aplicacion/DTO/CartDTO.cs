using Capa.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.DTO
{
    public class CartDTO
    {
        public ICollection<CartItemDTO> CartItems { get; set; }

    }
}
