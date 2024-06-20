using Capa.Datos.Entidades;

namespace Capa.Aplicacion.Repositorios.Interfaces
{
    public interface IOrdenRepositorio
    {
        Task<IEnumerable<Orden>> GetAllById(string userId);
        Task<Orden> Get(Guid ordenId);
        Task<Orden> Create(Orden orden);
        Task SaveChangesAsync();
    }
}
