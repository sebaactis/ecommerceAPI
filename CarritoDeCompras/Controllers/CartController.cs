using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.AspNetCore.Mvc;
using Capa.Aplicacion.DTI;
using Microsoft.AspNetCore.Authorization;
using Capa.Infraestructura.Servicios.Utilidades;
using Capa.Datos.Modelos;
using Azure;


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

        [HttpGet("getCart")]
        [Authorize]
        public async Task<ActionResult<CartDTO>> Get()
        {
            ApiResponse<CartDTO> response;

            try
            {
                var userId = _httpAccesor.getUserIdToken();

                if (userId == null)
                {
                    response = ApiResponse<CartDTO>.ErrorResponse(404, "Usuario no encontrado");
                    return NotFound(response);
                }

                var cart = await _cartService.GetCartById(userId);

                if (cart != null)
                {
                    var cartDto = _mapper.Map<CartDTO>(cart);
                    response = ApiResponse<CartDTO>.SuccessResponse(cartDto, 200);
                    return Ok(response);
                }

                response = ApiResponse<CartDTO>.ErrorResponse(404, "El usuario actual no tiene un carrito asociado");
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<CartDTO>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }

        }

        [HttpPost("newCart")]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ApiResponse<Cart> response;

            try
            {
                var userId = _httpAccesor.getUserIdToken();

                if (userId == null)
                {
                    response = ApiResponse<Cart>.ErrorResponse(404, "Usuario no encontrado");
                    return NotFound(response);
                }

                var cart = new Cart();
                cart.UserId = userId;

                await _cartService.createCart(cart);

                response = ApiResponse<Cart>.SuccessResponse(cart, 201, "Carrito creado correctamente!");
                return Ok(response);
            }

            catch (Exception ex)
            {
                response = ApiResponse<Cart>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("addProduct")]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] CartItemDTI cartItem)
        {
            ApiResponse<CartItemDTI> response;

            try
            {
                var userId = _httpAccesor.getUserIdToken();

                if (userId == null)
                {
                    response = ApiResponse<CartItemDTI>.ErrorResponse(404, "Usuario no encontrado");
                    return NotFound(response);
                }

                var cart = await _cartService.GetCartById(userId);

                if (cart == null)
                {
                    response = ApiResponse<CartItemDTI>.ErrorResponse(404, "Carrito no encontrado!");
                    return NotFound(response);
                }

                var item = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = cartItem.ProductId,
                    Cantidad = cartItem.Cantidad,
                };

                var itemDti = _mapper.Map<CartItemDTI>(item);

                await _cartService.AddProduct(item, userId);

                response = ApiResponse<CartItemDTI>.SuccessResponse(itemDti, 200, "Producto agregado al carrito correctamente");
                return Ok(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<CartItemDTI>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }

        }

        [HttpPost("resetCart")]
        [Authorize]
        public async Task<IActionResult> ResetCart()
        {
            ApiResponse<string> response;

            try
            {
                var userId = _httpAccesor.getUserIdToken();

                if (userId == null)
                {
                    response = ApiResponse<string>.ErrorResponse(404, "Usuario no encontrado");
                    return NotFound(response);
                }

                await _cartService.ResetCart(userId);
                response = ApiResponse<string>.SuccessResponse("Se vació el carrito", 200);
                return Ok(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<string>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpDelete("deleteProduct")]
        [Authorize]
        public async Task<IActionResult> Delete(int cartId, [FromBody] CartItemDTI cartItemDTI)
        {
            ApiResponse<CartItemDTI> response;

            try
            {
                var userId = _httpAccesor.getUserIdToken();

                if (userId == null)
                {
                    response = ApiResponse<CartItemDTI>.ErrorResponse(404, "Usuario no encontrado");
                    return NotFound(response);
                }

                var cartItem = _mapper.Map<CartItem>(cartItemDTI);

                await _cartService.RemoveProduct(cartItem, userId);

                response = ApiResponse<CartItemDTI>.SuccessResponse(cartItemDTI, 200, "Producto eliminado al carrito correctamente");

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<CartItemDTI>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }
    }
}
