using Capa.Infraestructura.Persistencia;
using Capa.Infraestructura.Repositorio.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Capa.Infraestructura.Repositorio.Implementacion
{
    public class RepositorioBase<T> : IRepositorioBase<T> where T : class
    {

        protected readonly CarritoDbContext _context;
        private readonly DbSet<T> dbSet;

        public RepositorioBase(CarritoDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity); 
        }

        public async Task Delete(int id)
        {
            var entity = await GetOne(id);

            if(entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = query.Include(includes);
            }

            if(!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetOne(int id)
        {
            var entity = await dbSet.FindAsync(id);

            if (entity != null) return entity;

            return null;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
    }
}
