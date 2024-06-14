using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.AspNetCore.Mvc;
using Capa.Aplicacion.DTI;
using Microsoft.AspNetCore.Authorization;
using Capa.Infraestructura.Servicios.Utilidades;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly HttpAccesor _httpAccesor;

        public CartController(ICartService cartService, IMapper mapper, HttpAccesor httpAccesor)
        {
            _cartService = cartService;
            _mapper = mapper;
            _httpAccesor = httpAccesor;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<CartDTO>> Get()
        {
            try
            {
                var userId = _httpAccesor.getUserIdToken();
                var cart = await _cartService.GetCartById(userId);

                if (cart != null)
                {
                    var cartDto = _mapper.Map<CartDTO>(cart);
                    return Ok(cartDto);
                }

                return NotFound("Carrito no encontrado!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("Crear")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            try
            {
                var userId = _httpAccesor.getUserIdToken();

                var cart = new Cart();
                cart.UserId = userId;

                await _cartService.createCart(cart);

                return Ok("Carrito creado correctamente");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AddProduct")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CartItemDTI cartItem)
        {
            try
            {
                var userId = _httpAccesor.getUserIdToken();
                var cart = await _cartService.GetCartById(userId);

                var item = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = cartItem.ProductId,
                    Cantidad = cartItem.Cantidad,
                };

                await _cartService.AddProduct(item, userId);

                return Ok("Producto agregado al carrito correctamente");

            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int cartId, [FromBody] CartItemDTI cartItemDTI)
        {
            try
            {
                var userId = _httpAccesor.getUserIdToken();

                var cartItem = _mapper.Map<CartItem>(cartItemDTI);

                await _cartService.RemoveProduct(cartItem, userId);

                return Ok("Producto elimado del carrito correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
