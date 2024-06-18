using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;

namespace Capa.Infraestructura.Servicios.Implementacion
{
    public class CartService : ICartService
    {
        private readonly ICartRepositorio _cartRepositorio;
        private readonly IProductService _productService;

        public CartService(ICartRepositorio cartRepositorio, IProductService productService)
        {
            _cartRepositorio = cartRepositorio;
            _productService = productService;
        }

        public async Task AddProduct(CartItem cartItem, string userId)
        {
            var prodExis = await _productService.GetOne(cartItem.ProductId, "ProductoId") ?? throw new Exception("El producto enviado no existe");

            var cartExis = await _cartRepositorio.GetCartById(userId) ?? throw new Exception("El carrito solicitado no existe");

            await _cartRepositorio.AddProduct(cartItem);
        }

        public async Task createCart(Cart cart)
        {
            var cartExist = await _cartRepositorio.GetCartById(cart.UserId) ?? throw new Exception("Ya existe un carrito activo para este usuario");

            cart.CreatedAt = DateTime.Now;
            cart.UpdatedAt = DateTime.Now;
            await _cartRepositorio.createCart(cart);
        }

        public async Task<Cart> GetCartById(string userId)
        {
            var cart = await _cartRepositorio.GetCartById(userId);

            if (cart != null) return cart;

            return null;
        }

        public async Task RemoveProduct(CartItem cartItem, string userId)
        {
            var prodExis = await _productService.GetOne(cartItem.ProductId, "ProductoId") ?? throw new Exception("El producto enviado no existe");

            var cartExis = await _cartRepositorio.GetCartById(userId) ?? throw new Exception("El carrito solicitado no existe");

            await _cartRepositorio.RemoveProduct(cartExis.CartId, cartItem);
        }

        public async Task ResetCart(string userId)
        {
            var cartExis = await _cartRepositorio.GetCartById(userId) ?? throw new Exception("El carrito solicitado no existe");
            cartExis.CartItems.Clear();

            await _cartRepositorio.ResetCart(cartExis);
        }
    }
}
