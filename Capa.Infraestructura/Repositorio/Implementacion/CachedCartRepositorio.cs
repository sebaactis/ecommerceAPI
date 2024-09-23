using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.Extensions.Caching.Memory;

namespace Capa.Infraestructura.Repositorio.Implementacion
{
    public class CachedCartRepositorio : ICartRepositorio
    {

        private readonly CartRepositorio _decorator;
        private readonly IMemoryCache _memoryCache;

        public CachedCartRepositorio(CartRepositorio decorator, IMemoryCache memoryCache)
        {
            _decorator = decorator;
            _memoryCache = memoryCache;
        }

        public Task AddProduct(CartItem cartItem)
        {
            return _decorator.AddProduct(cartItem);
        }

        public Task createCart(Cart cart)
        {
            return _decorator.createCart(cart);
        }

        public Task<Cart> GetCartById(string userId)
        {
            string key = $"cart-{userId}";

            return _memoryCache.GetOrCreateAsync(
                key,
                entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                    return _decorator.GetCartById(userId);
                });
        }

        public Task RemoveProduct(Guid cartId, CartItem cartItem)
        {
            return _decorator.RemoveProduct(cartId, cartItem);
        }

        public Task ResetCart(Cart cart)
        {
            return _decorator.ResetCart(cart);
        }

        public Task SaveChangesAsync()
        {
            return _decorator.SaveChangesAsync();
        }
    }
}
