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
    public class CategoriaController : ControllerBase
    {

        private readonly ICategoriaService _categoriaService;
        private readonly IMapper _mapper;

        public CategoriaController(ICategoriaService categoriaService, IMapper mapper)
        {
            _categoriaService = categoriaService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            try
            {
                var categorias = await _categoriaService.Get();

                var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

                return Ok(categoriasDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                var categoria = await _categoriaService.GetOne(id);

                if (categoria == null)
                {
                    return NotFound();
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

                return Ok(categoriaDto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CategoriaDTO categoria)
        {
            try
            {
                var newCategoria = new Categoria
                {
                    CategoriaId = categoria.CategoriaId,
                    Nombre = categoria.NombreCategoria,
                };

                await _categoriaService.Add(newCategoria);

                return Ok("Categoria creada correctamente!");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CategoriaDTI categoriaDTI)
        {
            try
            {
                if (categoriaDTI == null || categoriaDTI.NombreCategoria == null || id <= 0)
                {
                    return BadRequest();
                }

                try
                {
                    var categoriaFind = await _categoriaService.GetOne(id);

                    if (categoriaFind == null)
                    {
                        return NotFound();
                    }

                    var categoria = _mapper.Map<Categoria>(categoriaDTI);
                    await _categoriaService.Edit(id, categoria);

                    return Ok("Categoria editada correctamente");
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _categoriaService.Delete(id);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
