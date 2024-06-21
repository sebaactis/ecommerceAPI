using AutoMapper;
using Capa.Aplicacion.DTI;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Datos.Modelos;
using Capa.Infraestructura.Servicios.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {

        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductoController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> Get()
        {
            ApiResponse<IEnumerable<ProductoDTO>> response;

            try
            {
                var products = await _productService.Get(null, p => p.Categoria, true);

                if (products != null)
                {
                    var productosDto = _mapper.Map<IEnumerable<ProductoDTO>>(products);
                    response = ApiResponse<IEnumerable<ProductoDTO>>.SuccessResponse(productosDto, 200);
                    return Ok(response);
                }

                response = ApiResponse<IEnumerable<ProductoDTO>>.ErrorResponse(404, "No se pudieron recuperar los productos");
                return NotFound(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<IEnumerable<ProductoDTO>>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("GetOne")]
        public async Task<ActionResult<ProductoDTO>> Get(Guid id)
        {
            ApiResponse<ProductoDTO> response;

            try
            {

                var product = await _productService.GetOne(id, "ProductoId", p => p.Categoria);

                if (product != null)
                {
                    var productoDto = _mapper.Map<ProductoDTO>(product);
                    response = ApiResponse<ProductoDTO>.SuccessResponse(productoDto, 200);
                    return Ok(response);
                }

                response = ApiResponse<ProductoDTO>.ErrorResponse(404, "No se pudo recuperar el producto bajo ese ID");
                return NotFound(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<ProductoDTO>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("Crear")]
        [Authorize(Policy = "RequireAdm")]
        public async Task<IActionResult> Post([FromBody] ProductoDTI producto)
        {

            ApiResponse<ProductoDTI> response;
            try
            {
                var user = HttpContext.GetUserIdFromToken();

                if (user == null)
                {
                    response = ApiResponse<ProductoDTI>.ErrorResponse(401, "Usuario no autenticado");
                    return Unauthorized(response);
                }

                if (!ModelState.IsValid)
                {
                    var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                    response = ApiResponse<ProductoDTI>.ErrorResponse(400, error);
                    return BadRequest(response);
                }

                var newProducto = _mapper.Map<Producto>(producto);

                var result = await _productService.Add(newProducto);

                if (result != null)
                {
                    var productoDTO = _mapper.Map<ProductoDTI>(result);
                    response = ApiResponse<ProductoDTI>.SuccessResponse(productoDTO, 201);
                    return Ok(response);
                }

                response = ApiResponse<ProductoDTI>.ErrorResponse(400, "No se pudo crear el producto");
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<ProductoDTI>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPut("Editar")]
        [Authorize(Policy = "RequireAdm")]
        public async Task<ActionResult<ProductoDTI>> Put(Guid id, [FromBody] ProductoDTI productoDTI)
        {
            ApiResponse<ProductoDTI> response;
            try
            {
                var user = HttpContext.GetUserIdFromToken();

                if (user == null)
                {
                    response = ApiResponse<ProductoDTI>.ErrorResponse(401, "Usuario no autenticado");
                    return Unauthorized(response);
                }

                if (!ModelState.IsValid)
                {
                    var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                    response = ApiResponse<ProductoDTI>.ErrorResponse(400, error);
                    return BadRequest(response);
                }

                var productoFind = await _productService.GetOne(id, "ProductoId");

                if (productoFind == null)
                {
                    response = ApiResponse<ProductoDTI>.ErrorResponse(404, "Producto no encontrado");
                    return NotFound(response);
                }

                var producto = _mapper.Map<Producto>(productoDTI);
                var result = await _productService.Edit(id, producto);

                if (result != null)
                {
                    var productoDTO = _mapper.Map<ProductoDTI>(result);
                    response = ApiResponse<ProductoDTI>.SuccessResponse(productoDTO, 200);
                    return Ok(response);
                }

                response = ApiResponse<ProductoDTI>.ErrorResponse(400, "No se pudo editar el producto");
                return BadRequest(response);
            }

            catch (Exception ex)
            {
                response = ApiResponse<ProductoDTI>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }

        }

        [HttpDelete("Eliminar")]
        [Authorize(Policy = "RequireAdm")]
        public async Task<IActionResult> Delete(Guid id)
        {
            ApiResponse<ProductoDTI> response;

            try
            {
                var user = HttpContext.GetUserIdFromToken();

                if (user == null)
                {
                    response = ApiResponse<ProductoDTI>.ErrorResponse(401, "Usuario no autenticado");
                    return Unauthorized(response);
                }

                var productoFind = await _productService.GetOne(id, "ProductoId");

                if (productoFind == null)
                {
                    response = ApiResponse<ProductoDTI>.ErrorResponse(404, "Producto no encontrado");
                    return NotFound(response);
                }

                var result = await _productService.Delete(id, "ProductoId");

                if (result != null)
                {
                    var productoDTO = _mapper.Map<ProductoDTI>(result);
                    response = ApiResponse<ProductoDTI>.SuccessResponse(productoDTO, 200, "Producto eliminado correctamente");
                    return Ok(response);
                }

                response = ApiResponse<ProductoDTI>.ErrorResponse(400, "No se pudo eliminar el producto");
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<ProductoDTI>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }
    }
}
