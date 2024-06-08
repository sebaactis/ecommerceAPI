using AutoMapper;
using Capa.Aplicacion.DTI;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
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

                var product = await _productService.GetOne(id);

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
        public async void Post([FromBody] ProductoDTI producto)
        {
            var newProducto = _mapper.Map<Producto>(producto);
            await _productService.Add(newProducto);

            Ok("Producto creado correctamente!");
        }

        // PUT api/<ProductoController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductoController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
