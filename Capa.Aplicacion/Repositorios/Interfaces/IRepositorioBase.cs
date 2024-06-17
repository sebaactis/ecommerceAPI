using System.Linq.Expressions;

namespace Capa.Infraestructura.Repositorio.Interfaces
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true);
        Task<T> GetOne(int id, string? property, Expression<Func<T, object>>? includes = null);
        Task<T> Add(T entity);
        Task Delete(int id, string property);
        Task Update(T entity);
        Task SaveChangesAsync();

    }
}
