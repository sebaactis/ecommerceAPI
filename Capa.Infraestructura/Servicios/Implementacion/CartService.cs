using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task AddProduct(CartItem cartItem)
        {
            var prodExis = await _productService.GetOne(cartItem.ProductId);

            if (prodExis == null) return;

            var cartExis = await _cartRepositorio.GetCartById(cartItem.CartId);

            if(cartExis == null) return;

            await _cartRepositorio.AddProduct(cartItem);
        }

        public async Task createCart(Cart cart)
        {
            var cartExist = await _cartRepositorio.GetCartById(cart.CartId);

            if (cartExist != null) return;

            cart.CreatedAt = DateTime.Now;
            cart.UpdatedAt = DateTime.Now;
            await _cartRepositorio.createCart(cart);
        }

        public async Task<Cart> GetCartById(int cartId)
        {
            var cart =  await _cartRepositorio.GetCartById(cartId);

            if(cart != null) return cart;

            return null;
        }

        public Task<Cart> RemoveProduct(int cartId, CartItem cartItem)
        {
            throw new NotImplementedException();
        }

        public Task<Cart> RemoveQuantityProduct(int cartId, CartItem cartItem)
        {
            throw new NotImplementedException();
        }
    }
}
