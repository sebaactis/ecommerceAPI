using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

        public async Task AddProduct(CartItem cartItem, string userId)
        {
            var prodExis = await _productService.GetOne(cartItem.ProductId);

            if (prodExis == null) return;

            var cartExis = await _cartRepositorio.GetCartById(userId);

            if (cartExis == null) return;

            await _cartRepositorio.AddProduct(cartItem);

        }

        public async Task createCart(Cart cart)
        {
            var cartExist = await _cartRepositorio.GetCartById(cart.UserId);

            if (cartExist != null) return;

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
            var prodExis = await _productService.GetOne(cartItem.ProductId);

            if (prodExis == null) return;

            var cartExis = await _cartRepositorio.GetCartById(userId);

            if (cartExis == null) return;

            await _cartRepositorio.RemoveProduct(cartExis.CartId, cartItem);
        }

        public async Task ResetCart(string userId)
        {
            var cartExis = await _cartRepositorio.GetCartById(userId);

            if (cartExis == null) return;

            cartExis.CartItems.Clear();

            await _cartRepositorio.ResetCart(cartExis);
        }
    }
}
