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

        public async Task<T> Add(T entity)
        {
            var result = await dbSet.AddAsync(entity);

            if (result.State == EntityState.Added)
            {
               
                await SaveChangesAsync();
                return result.Entity;
            }

            return null;
        }

        public async Task<T> Delete(int id, string property)
        {
            var entity = await GetOne(id, property);

            if (entity != null)
            {
                var result = dbSet.Remove(entity);

                if (result.State == EntityState.Deleted)
                {
                    await SaveChangesAsync();
                    return entity;
                }

                return null;    
            }

            return null;
        }

        public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, Expression<Func<T, object>>? includes = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includes != null)
            {
                query = query.Include(includes);
            }

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            return await query.ToListAsync();
        }

        public async Task<T> GetOne(int id, string? property, Expression<Func<T, object>>? includes = null)
        {
            IQueryable<T> query = dbSet;

            if (includes != null)
            {
                query = query.Include(includes);
            }

            if (property != null)
            {
                var entity = await query.FirstOrDefaultAsync(e => EF.Property<int>(e, property) == id);

                if (entity != null) return entity;
            }
            return null;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<T> Update(T entity)
        {
            var result = dbSet.Update(entity);

            if (result.State == EntityState.Modified)
            {
                await SaveChangesAsync();
                return entity;
            }

            return null;
        }
    }
}
