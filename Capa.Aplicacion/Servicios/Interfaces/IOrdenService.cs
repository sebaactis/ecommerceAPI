using Capa.Datos.Entidades;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface IOrdenService
    {
        Task<IEnumerable<Orden>> GetAllById(string userId);
        Task<Orden> Get(Guid ordenId);
        Task<Orden> Create(string userId);
    }
}
