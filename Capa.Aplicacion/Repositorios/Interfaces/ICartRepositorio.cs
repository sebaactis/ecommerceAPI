using Capa.Datos.Entidades;

namespace Capa.Aplicacion.Repositorios.Interfaces
{
    public interface ICartRepositorio
    {
        Task createCart(Cart cart);
        Task<Cart> GetCartById(string userId);
        Task AddProduct(CartItem cartItem);
        Task RemoveProduct(Guid cartId, CartItem cartItem);
        Task ResetCart(Cart cart);
        Task SaveChangesAsync();
    }
}
