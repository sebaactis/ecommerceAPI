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
            producto.CreatedAt = DateTime.Now;
            producto.UpdatedAt = DateTime.Now;
            await _repositorio.Add(producto);
            await _repositorio.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var product = await _repositorio.GetOne(id);

            if (product != null)
            {
                await _repositorio.Delete(id);
            }
        }

        public async Task Edit(int id, Producto producto)
        {
            var productoFind = await _repositorio.GetOne(id);
            if (productoFind != null)
            {
                productoFind.Nombre = producto.Nombre;
                productoFind.Descripcion = producto.Descripcion;
                productoFind.Precio = producto.Precio;
                productoFind.Stock = producto.Stock;
                productoFind.CategoriaId = producto.CategoriaId;
                productoFind.UpdatedAt = DateTime.Now;

                await _repositorio.Update(productoFind);
            }
        }

        public async Task<IEnumerable<Producto>> Get(Expression<Func<Producto, bool>>? filter = null, Expression<Func<Producto, object>>? includes = null, bool tracked = true)
        {
            var products = await _repositorio.GetAll(filter, includes, tracked);
            return products;
        }

        public Task<Producto> GetOne(int id, Expression<Func<Producto, object>>? includes = null)
        {
            var product = _repositorio.GetOne(id, includes);

            if (product != null) return product;

            return null;
        }
    }
}
