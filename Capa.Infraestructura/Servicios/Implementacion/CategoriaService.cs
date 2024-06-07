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
            await _repositorio.Add(categoria);
            await _repositorio.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task Edit(Categoria T)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Categoria>> Get(Expression<Func<Categoria, bool>>? filter = null, Expression<Func<Categoria, object>>? includes = null, bool tracked = true)
        {
            throw new NotImplementedException();
        }

        public Task<Categoria> GetOne(int id)
        {
            throw new NotImplementedException();
        }
    }
}
