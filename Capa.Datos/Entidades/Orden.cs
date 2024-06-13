using Microsoft.AspNetCore.Identity;
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
        public string UserId { get; set; }
        public User User { get; set; }
        public decimal Total { get; set; }
        public ICollection<OrdenItem> OrdenItems { get; set; }
    }
}
