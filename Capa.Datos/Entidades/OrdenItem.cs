using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Datos.Entidades
{
    public class OrdenItem
    {
        public int OrdenItemId { get; set; }
        public int OrdenId { get; set; }
        public Orden Orden { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }
    }
}
