using System.Linq.Expressions;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface IService<T> where T : class
    {
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true);
        Task<T> GetOne(Guid id, string? property, Expression<Func<T, object>>? includes = null);
        Task<T> Add(T T);
        Task<T> Edit(Guid id, T T);
        Task<T> Delete(Guid id, string? property);
    }
}
