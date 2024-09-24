using Capa.Infraestructura.Repositorio.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Expressions;


namespace Capa.Infraestructura.Repositorio.Implementacion
{
    public class CachedRepositorioBase<T> : IRepositorioBase<T> where T : class
    {

        private readonly RepositorioBase<T> _decorator;
        private readonly IMemoryCache _memoryCache;

        public CachedRepositorioBase(RepositorioBase<T> decorator, IMemoryCache memoryCache)
        {
            _decorator = decorator;
            _memoryCache = memoryCache;
        }

        public Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<T> Delete(Guid id, string property)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true)
        {
            var key = Guid.NewGuid().ToString();

            return _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _decorator.GetAll(filter, includes, tracked);
                }
                );
        }

        public Task<T> GetOne(Guid id, string? property, Expression<Func<T, object>>? includes = null)
        {
            var key = Guid.NewGuid().ToString();

            return _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
                    return _decorator.GetOne(id, property, includes);
                }
                );
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<T> Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
