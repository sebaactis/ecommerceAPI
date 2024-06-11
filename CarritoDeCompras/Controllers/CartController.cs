using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> Add(int cartId, [FromBody] CartItemDTO cartItem)
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

        // PUT api/<CartController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CartController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
