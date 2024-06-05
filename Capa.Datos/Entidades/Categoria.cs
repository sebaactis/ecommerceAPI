using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Datos.Entidades
{
    public class Categoria : ModelBase
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; }
        public ICollection<Producto> Productos { get; set; }

    }
}
