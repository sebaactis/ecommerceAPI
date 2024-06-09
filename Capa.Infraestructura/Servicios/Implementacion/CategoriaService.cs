using Capa.Aplicacion.DTI;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Repositorio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Infraestructura.Servicios.Implementacion
{
    public class CategoriaService : ICategoriaService
    {

        private readonly IRepositorioBase<Categoria> _repositorio;

        public CategoriaService(IRepositorioBase<Categoria> repositorioBase)
        {
            _repositorio = repositorioBase;
        }

        public async Task Add(Categoria categoria)
        {
            categoria.CreatedAt = DateTime.Now;
            categoria.UpdatedAt = DateTime.Now;
            await _repositorio.Add(categoria);
        }

        public async Task Delete(int id)
        {
            await _repositorio.Delete(id);
        }

        public async Task Edit(int id, Categoria categoria)
        {
            var categoriaFind = await _repositorio.GetOne(id);
            if (categoriaFind != null)
            {
                categoriaFind.Nombre = categoria.Nombre;
                categoriaFind.UpdatedAt = DateTime.Now;

                await _repositorio.Update(categoriaFind);
            }       

        }

        public async Task<IEnumerable<Categoria>> Get(Expression<Func<Categoria, bool>>? filter = null, Expression<Func<Categoria, object>>? includes = null, bool tracked = true)
        {
            return await _repositorio.GetAll(filter, includes, tracked);
        }

        public Task<Categoria> GetOne(int id, Expression<Func<Categoria, object>>? includes = null)
        {
            var categoria = _repositorio.GetOne(id, includes);

            if (categoria == null) return null;

            return categoria;

        }
    }
}
