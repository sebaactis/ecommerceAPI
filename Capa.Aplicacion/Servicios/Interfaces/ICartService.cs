using Capa.Datos.Entidades;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface ICartService
    {
        Task createCart(Cart cart);
        Task<Cart> GetCartById(string userId);
        Task AddProduct(CartItem cartItem, string userId);
        Task RemoveProduct(CartItem cartItem, string userId);
        Task ResetCart(string userId);
    }
}
