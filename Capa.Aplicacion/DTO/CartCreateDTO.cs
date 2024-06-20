using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.DTO
{
    public class CartCreateDTO
    {
        public Guid CartId { get; set; }
        public string UserId { get; set; }
    }
}
