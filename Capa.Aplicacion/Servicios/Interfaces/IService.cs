using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true);
        Task<T> GetOne(int id, Expression<Func<T, object>>? includes = null);
        Task Add(T T);
        Task Edit(int id, T T);
        Task Delete(int id);
    }
}
