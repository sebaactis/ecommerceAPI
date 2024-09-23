using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Capa.Infraestructura.Repositorio.Implementacion
{
    public class CartRepositorio : ICartRepositorio
    {

        protected readonly CarritoDbContext _context;
        private readonly DbSet<Cart> dbSetCart;
        private readonly DbSet<CartItem> dbSetCartItem;

        public CartRepositorio(CarritoDbContext context)
        {
            _context = context;
            this.dbSetCart = _context.Set<Cart>();
            this.dbSetCartItem = _context.Set<CartItem>();
        }

        public async Task AddProduct(CartItem cartItem)
        {
            var existingCart = await dbSetCart.Include(c => c.CartItems)
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
            await dbSetCart.AddAsync(cart);
            await SaveChangesAsync();
        }

        public async Task<Cart> GetCartById(string userId)
        {

            var cart = await dbSetCart.Include(c => c.User).Include(c => c.CartItems).ThenInclude(ci => ci.Producto).ThenInclude(p => p.Categoria).FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart != null) return cart;

            return null;
        }

        public async Task RemoveProduct(Guid cartId, CartItem cartItem)
        {
            var existingCart = await dbSetCart.Include(c => c.CartItems)
                                  .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (existingCart == null) return;

            var existingCartItem = existingCart.CartItems.FirstOrDefault(ci => ci.ProductId == cartItem.ProductId);

            if (existingCartItem == null) return;

            cartItem.CartItemId = existingCartItem.CartItemId;

            if (existingCartItem.Cantidad > cartItem.Cantidad)
            {
                existingCartItem.Cantidad -= cartItem.Cantidad;
            }
            else
            {
                existingCart.CartItems.Remove(existingCartItem);

                dbSetCartItem.Remove(existingCartItem);
            }

            await SaveChangesAsync();
        }

        public async Task ResetCart(Cart cart)
        {
            dbSetCart.Update(cart);
            await SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
