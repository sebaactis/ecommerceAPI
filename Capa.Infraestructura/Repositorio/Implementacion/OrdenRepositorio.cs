using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


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

        public async Task Create(Orden orden)
        {
            await dbSet.AddAsync(orden);
            await SaveChangesAsync();
        }

        public async Task<Orden> Get(int ordenId)
        {

            var orden = await dbSet.Include(o => o.OrdenItems).ThenInclude(oi => oi.Producto).ThenInclude(p => p.Categoria).FirstOrDefaultAsync(o => o.OrdenId == ordenId);

            if (orden == null) return null;

            return orden;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
