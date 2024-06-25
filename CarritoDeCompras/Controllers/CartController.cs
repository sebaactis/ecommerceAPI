using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.AspNetCore.Mvc;
using Capa.Aplicacion.DTI;
using Microsoft.AspNetCore.Authorization;
using Capa.Infraestructura.Servicios.Utilidades;
using Capa.Datos.Modelos;

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet("getCart")]
        public async Task<ActionResult<CartDTO>> Get()
        {
            ApiResponse<CartDTO> response;

            try
            {
                var userId = HttpContext.GetUserIdFromToken();

                if (userId == null)
                {
                    response = ApiResponse<CartDTO>.ErrorResponse(401, "Usuario no autenticado");
                    return Unauthorized(response);
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
        public async Task<IActionResult> Create()
        {
            ApiResponse<CartCreateDTO> response;

            try
            {
                var userId = UserValidation.userValidationId(HttpContext);

                if(userId == null)
                {
                    response = ApiResponse<CartCreateDTO>.ErrorResponse(404, "Usuario no encontrado");
                   return NotFound(response);
                }

                var cart = new Cart();
                cart.UserId = userId;
                cart.CartId = Guid.NewGuid();

                await _cartService.createCart(cart);

                var cartDto = _mapper.Map<CartCreateDTO>(cart);
                response = ApiResponse<CartCreateDTO>.SuccessResponse(cartDto, 201, "Carrito creado correctamente!");
                return Ok(response);
            }

            catch (Exception ex)
            {
                response = ApiResponse<CartCreateDTO>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("addProduct")]
        public async Task<IActionResult> Add([FromBody] CartItemDTI cartItem)
        {
            ApiResponse<CartItemDTI> response;

            try
            {
                var userId = UserValidation.userValidationId(HttpContext);

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
        public async Task<IActionResult> ResetCart()
        {
            ApiResponse<string> response;

            try
            {
                var userId = UserValidation.userValidationId(HttpContext);

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
        public async Task<IActionResult> Delete(int cartId, [FromBody] CartItemDTI cartItemDTI)
        {
            ApiResponse<CartItemDTI> response;

            try
            {
                var userId = UserValidation.userValidationId(HttpContext);

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
