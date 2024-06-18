using System.Linq.Expressions;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true);
        Task<T> GetOne(int id, string? property, Expression<Func<T, object>>? includes = null);
        Task<T> Add(T T);
        Task<T> Edit(int id, T T);
        Task<T> Delete(int id, string? property);
    }
}
