using Capa.Aplicacion.Repositorios.Interfaces;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;

namespace Capa.Infraestructura.Servicios.Implementacion
{
    public class OrdenService : IOrdenService
    {
        private readonly IOrdenRepositorio _ordenRepositorio;
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public OrdenService(IOrdenRepositorio ordenRepositorio, IProductService productService, ICartService cartService)
        {
            _ordenRepositorio = ordenRepositorio;
            _productService = productService;
            _cartService = cartService;
        }

        public async Task<Orden> Create(string userId)
        {
            var cart = await _cartService.GetCartById(userId);

            if (cart == null) return null;

            var cartItems = cart.CartItems;

            if (cartItems.Count() <= 0) throw new Exception("El carrito actual se encuentra vacio");

            Orden orden = new Orden
            {
                OrdenDate = DateTime.Now,
                OrdenItems = new List<OrdenItem>(),
                UserId = cart.UserId,
                Total = 0
            };

            foreach (var cartItem in cartItems)
            {
                var product = await _productService.GetOne(cartItem.ProductId, "ProductoId");

                if (product.Stock > 0 && product.Stock >= cartItem.Cantidad)
                {
                    OrdenItem ordenItem = new OrdenItem
                    {
                        ProductoId = cartItem.ProductId,
                        Cantidad = cartItem.Cantidad,
                        PrecioUnitario = product.Precio
                    };

                    product.Stock -= cartItem.Cantidad;
                    await _productService.Edit(product.ProductoId, product);
                    orden.OrdenItems.Add(ordenItem);
                    orden.Total += (ordenItem.Cantidad * ordenItem.PrecioUnitario);
                }
                else
                {
                    throw new Exception("Unos de los productos supera el stock actual, por favor, verificar");
                }
            };

            await _cartService.ResetCart(cart.UserId);
            var result = await _ordenRepositorio.Create(orden);

            if (result != null) return result;

            throw new Exception("Ocurrió un error al intentar crear su orden, intente de nuevo más tardes");
        }

        public async Task<Orden> Get(Guid ordenId)
        {
            var orden = await _ordenRepositorio.Get(ordenId);

            if (orden == null) return null;

            return orden;
        }

        public async Task<IEnumerable<Orden>> GetAllById(string userId)
        {
            var result = await _ordenRepositorio.GetAllById(userId);

            if (result != null) return result;

            return null;
        }
    }
}
