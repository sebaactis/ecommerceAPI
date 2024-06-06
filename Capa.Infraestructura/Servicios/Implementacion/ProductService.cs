using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Repositorio.Interfaces;
using System.Linq.Expressions;

namespace Capa.Aplicacion.Servicios.Implementacion
{

    public class ProductService : IProductService
    {
        private readonly IRepositorioBase<Producto> _repositorio;
        public ProductService(IRepositorioBase<Producto> repositorio)
        {
            _repositorio = repositorio;
        }

        public async Task Add(Producto producto)
        {
            await _repositorio.Add(producto);
            await _repositorio.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = await _repositorio.GetOne(id);

            if(product != null)
            {
                await _repositorio.Delete(id);
            }
        }

        public async Task Edit(Producto producto)
        {
            _repositorio.Update(producto);
        }

        public async Task<IEnumerable<Producto>> Get(Expression<Func<Producto, bool>>? filter = null, Expression<Func<Producto, object>>? includes = null, bool tracked = true)
        {
            var products = await _repositorio.GetAll(filter, includes, tracked);
            return products;
        }

        public Task<Producto> GetOne(int id)
        {
            var product = _repositorio.GetOne(id);

            return product;
        }
    }
}
