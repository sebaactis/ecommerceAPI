using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Capa.Infraestructura.Repositorio.Implementacion
{
    public class OrdenRepositorio : IOrdenRepositorio
    {
        protected readonly CarritoDbContext _context;
        private readonly DbSet<Orden> dbSet;

        public OrdenRepositorio(CarritoDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<Orden>();
        }

        public async Task<Orden> Create(Orden orden)
        {
            var ordenCreate = await dbSet.AddAsync(orden);

            if (ordenCreate.State == EntityState.Added)
            {
                await SaveChangesAsync();

                return orden;
            }

            return null;
        }

        public async Task<Orden> Get(Guid ordenId)
        {

            var orden = await dbSet.Include(o => o.User).Include(o => o.OrdenItems).ThenInclude(oi => oi.Producto).ThenInclude(p => p.Categoria).FirstOrDefaultAsync(o => o.OrdenId == ordenId);

            if (orden == null) return null;

            return orden;
        }

        public async Task<IEnumerable<Orden>> GetAllById(string userId)
        {
            var ordenes = await dbSet
                .Where(o => o.UserId == userId)
                .Include(o => o.User)
                .Include(o => o.OrdenItems)
                .ThenInclude(oi => oi.Producto)
                .ThenInclude(p => p.Categoria)
                .ToListAsync();

            if (ordenes.Count == 0) return null;

            return ordenes;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
