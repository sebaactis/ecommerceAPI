using Capa.Aplicacion.DTO;
using Capa.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface ICartService
    {
        Task createCart(Cart cart);
        Task<Cart> GetCartById(int cartId);
        Task AddProduct(CartItem cartItem);
        Task<Cart> RemoveProduct(int cartId, CartItem cartItem);
        Task<Cart> RemoveQuantityProduct(int cartId, CartItem cartItem);
    }
}
