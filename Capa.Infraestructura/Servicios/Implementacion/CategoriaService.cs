using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Repositorio.Interfaces;
using System.Linq.Expressions;

namespace Capa.Infraestructura.Servicios.Implementacion
{
    public class CategoriaService : ICategoriaService
    {

        private readonly IRepositorioBase<Categoria> _repositorio;

        public CategoriaService(IRepositorioBase<Categoria> repositorioBase)
        {
            _repositorio = repositorioBase;
        }

        public async Task<Categoria> Add(Categoria categoria)
        {
            var categorias = await _repositorio.GetAll();

            var categoriaNew = categorias.FirstOrDefault(c => c.Nombre == categoria.Nombre);

            if (categoriaNew != null) throw new Exception("Ya existe una categoria con ese nombre");

            categoria.CreatedAt = DateTime.Now;
            categoria.UpdatedAt = DateTime.Now;
            var result = await _repositorio.Add(categoria);

            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<Categoria> Delete(Guid id, string property)
        {
            var result = await _repositorio.Delete(id, property);

            if (result != null)
            {
                return result;
            }

            return null;
        }

        public async Task<Categoria> Edit(Guid id, Categoria categoria)
        {
            var categoriaFind = await _repositorio.GetOne(id, "CategoriaId");

            if (categoriaFind != null)
            {
                categoriaFind.Nombre = categoria.Nombre;
                categoriaFind.UpdatedAt = DateTime.Now;

                var result = await _repositorio.Update(categoriaFind);

                if (result != null) return result;

                return null;
            }
            return null;
        }

        public async Task<IEnumerable<Categoria>> Get(Expression<Func<Categoria, bool>>? filter = null, Expression<Func<Categoria, object>>? includes = null, bool tracked = true)
        {
            var categorias = await _repositorio.GetAll(filter, includes, tracked) ?? throw new Exception("No pudimos recuperar las categorias");

            return categorias;
        }

        public Task<Categoria> GetOne(Guid id, string? property, Expression<Func<Categoria, object>>? includes = null)
        {
            var categoria = _repositorio.GetOne(id, property, includes) ?? null;

            return categoria;
        }
    }
}
