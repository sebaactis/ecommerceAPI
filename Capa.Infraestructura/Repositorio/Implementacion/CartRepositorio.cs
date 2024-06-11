using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Capa.Infraestructura.Repositorio.Implementacion
{
    public class CartRepositorio : ICartRepositorio
    {

        protected readonly CarritoDbContext _context;
        private readonly DbSet<Cart> dbSet;

        public CartRepositorio(CarritoDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<Cart>();
        }

        public async Task AddProduct(CartItem cartItem)
        {
            var existingCart = await dbSet.Include(c => c.CartItems)
                                  .FirstOrDefaultAsync(c => c.CartId == cartItem.CartId);

            if (existingCart == null) return;

            var existingCartItem = existingCart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);

            if (existingCartItem != null)
            {
                existingCartItem.Cantidad += cartItem.Cantidad;
            }
            else
            {
                existingCart.CartItems.Add(cartItem);
            }

            await SaveChangesAsync();
        }

        public async Task createCart(Cart cart)
        {
            await dbSet.AddAsync(cart);
            await SaveChangesAsync();
        }

        public async Task<Cart> GetCartById(int cartId)
        {

            var cart = await dbSet.Include(c => c.CartItems).ThenInclude(ci => ci.Producto).ThenInclude(p => p.Categoria).FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart != null) return cart;

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

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
