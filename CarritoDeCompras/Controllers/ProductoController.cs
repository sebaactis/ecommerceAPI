using AutoMapper;
using Capa.Aplicacion.DTI;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Infraestructura.Servicios.Implementacion;
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

        [HttpGet]
        public async Task<IEnumerable<ProductoDTO>> Get()
        {
            try
            {
                var products = await _productService.Get(null, p => p.Categoria, true);

                var productosDto = _mapper.Map<IEnumerable<ProductoDTO>>(products);

                return productosDto;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var product = await _productService.GetOne(id, p => p.Categoria);

                if (product != null)
                {
                    var productoDto = _mapper.Map<ProductoDTO>(product);
                    return Ok(productoDto);
                }

                return NoContent();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductoDTI producto)
        {
            var newProducto = _mapper.Map<Producto>(producto);
            await _productService.Add(newProducto);

            return Ok("Producto creado correctamente!");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductoDTI productoDTI)
        {
            try
            {
                if (productoDTI == null || id <= 0)
                {
                    return BadRequest();
                }

                try
                {
                    var productoFind = await _productService.GetOne(id);

                    if (productoFind == null)
                    {
                        return NotFound();
                    }

                    var producto = _mapper.Map<Producto>(productoDTI);
                    await _productService.Edit(id, producto);

                    return Ok("Producto editado correctamente");
                }

                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<ProductoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var productoFind = await _productService.GetOne(id);

                if (productoFind == null)
                {
                    return NotFound("No existe un producto con ese id");
                }

                await _productService.Delete(id);

                return Ok("Producto eliminado correctamente!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
