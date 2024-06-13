using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using System.Text.Json;
using Capa.Aplicacion.DTI;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;


        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CartDTO>> Get(int cartId)
        {
            try
            {
                var cart = await _cartService.GetCartById(cartId);

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

        [HttpPost]
        [Route("Crear")]
        public async Task<IActionResult> Create()
        {
            try
            {
                var cart = new Cart();

                await _cartService.createCart(cart);

                return Ok("Carrito creado correctamente");
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<IActionResult> Add(int cartId, [FromBody] CartItemDTI cartItem)
        {
            try
            {
                var item = new CartItem
                {
                    CartId = cartId,
                    ProductId = cartItem.ProductId,
                    Cantidad = cartItem.Cantidad,
                };
                
                await _cartService.AddProduct(item);

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

                var cartItem = _mapper.Map<CartItem>(cartItemDTI);

                await _cartService.RemoveProduct(cartId, cartItem);

                return Ok("Producto elimado del carrito correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

        }
    }
}
