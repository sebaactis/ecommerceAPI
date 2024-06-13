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

        public async Task Create(int cartId)
        {
            var cart = await _cartService.GetCartById(cartId);
            var cartItems = cart.CartItems;

            Orden orden = new Orden
            {
                OrdenDate = DateTime.Now,
                OrdenItems = new List<OrdenItem>(),
                Total = 0
            };

            foreach (var cartItem in cartItems)
            {
                var product = await _productService.GetOne(cartItem.ProductId);

                OrdenItem ordenItem = new OrdenItem
                {
                    ProductoId = cartItem.ProductId,
                    Cantidad = cartItem.Cantidad,
                    PrecioUnitario = product.Precio
                };

                orden.OrdenItems.Add(ordenItem);
                orden.Total += (ordenItem.Cantidad * ordenItem.PrecioUnitario); 
            };

            await _ordenRepositorio.Create(orden);
        }

        public async Task<Orden> Get(int ordenId)
        {
            var orden = await _ordenRepositorio.Get(ordenId);

            if (orden == null) return null;

            return orden;
        }
    }
}
