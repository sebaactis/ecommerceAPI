using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Capa.Infraestructura.Repositorio.Implementacion
{
    public class CachedCartRepositorio : ICartRepositorio
    {

        private readonly CartRepositorio _decorator;
        private readonly IDistributedCache _distributedCache;

        public CachedCartRepositorio(CartRepositorio decorator, IDistributedCache distributedCache)
        {
            _decorator = decorator;
            _distributedCache = distributedCache;
        }

        public Task AddProduct(CartItem cartItem)
        {
            return _decorator.AddProduct(cartItem);
        }

        public Task createCart(Cart cart)
        {
            return _decorator.createCart(cart);
        }

        public async Task<Cart> GetCartById(string userId)
        {
            string key = $"cart-{userId}";

            string? cachedCart = await _distributedCache.GetStringAsync(
                key);

            Cart cart;
            if (string.IsNullOrEmpty(cachedCart))
            {
                cart = await _decorator.GetCartById(userId);

                if (cart is null)
                {
                    return cart;
                }

                await _distributedCache.SetStringAsync(key, JsonConvert.SerializeObject(cart, new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(2)
                });

                return cart;
            }

            cart = JsonConvert.DeserializeObject<Cart>(cachedCart);

            return cart;

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
