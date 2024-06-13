using Capa.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface IOrdenService
    {
        Task<Orden> Get(int ordenId);
        Task Create(int cartId);
    }
}
