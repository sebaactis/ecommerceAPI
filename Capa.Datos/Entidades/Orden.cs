using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Datos.Entidades
{
    public class Orden
    {
        public int OrdenId { get; set; }
        public DateTime OrdenDate { get; set; }
        public ICollection<OrdenItem> OrdenItems { get; set; }
    }
}
