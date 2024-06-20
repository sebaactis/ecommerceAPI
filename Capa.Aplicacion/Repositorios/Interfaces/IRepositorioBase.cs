using System.Linq.Expressions;

namespace Capa.Infraestructura.Repositorio.Interfaces
{
    public interface IRepositorioBase<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true);
        Task<T> GetOne(Guid id, string? property, Expression<Func<T, object>>? includes = null);
        Task<T> Add(T entity);
        Task<T> Delete(Guid id, string property);
        Task<T> Update(T entity);
        Task SaveChangesAsync();

    }
}
