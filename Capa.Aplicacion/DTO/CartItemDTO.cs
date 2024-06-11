using Capa.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.DTO
{
    public class CartItemDTO
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public ProductoDTO Producto { get; set; }
        public int Cantidad { get; set; }
    }
}
