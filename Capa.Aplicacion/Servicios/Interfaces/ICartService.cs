using Capa.Aplicacion.DTO;
using Capa.Datos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Aplicacion.Servicios.Interfaces
{
    public interface ICartService
    {
        Task createCart(Cart cart);
        Task<Cart> GetCartById(string userId);
        Task AddProduct(CartItem cartItem, string userId);
        Task RemoveProduct(CartItem cartItem, string userId);
    }
}
