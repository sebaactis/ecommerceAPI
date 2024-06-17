using AutoMapper;
using Capa.Aplicacion.DTI;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Capa.Datos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;


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

        [HttpGet("GetAll")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get()
        {
            ApiResponse<IEnumerable<CategoriaDTO>> response;
            try
            {
                var categorias = await _categoriaService.Get();
                var categoriasDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);
                response = ApiResponse<IEnumerable<CategoriaDTO>>.SuccessResponse(categoriasDto, 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<IEnumerable<CategoriaDTO>>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("GetOne")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            ApiResponse<CategoriaDTO> response;

            try
            {
                var categoria = await _categoriaService.GetOne(id, "CategoriaId");

                if (categoria == null)
                {
                    response = ApiResponse<CategoriaDTO>.ErrorResponse(404, "No se encuentra la categoria con ese ID");
                    return NotFound(response);
                }

                var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);
                response = ApiResponse<CategoriaDTO>.SuccessResponse(categoriaDto, 200);

                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<CategoriaDTO>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }

        }

        [HttpPost("Create")]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CategoriaDTO categoria)
        {
            ApiResponse<CategoriaDTO> response;

            try
            {
                var newCategoria = new Categoria
                {
                    CategoriaId = categoria.CategoriaId,
                    Nombre = categoria.NombreCategoria,
                };

                var result = await _categoriaService.Add(newCategoria);

                if (result != null)
                {
                    var categoriaDto = _mapper.Map<CategoriaDTO>(result);
                    response = ApiResponse<CategoriaDTO>.SuccessResponse(categoriaDto, 201, "Categoria creada correctamente");
                    return Ok(response);
                }

                response = ApiResponse<CategoriaDTO>.ErrorResponse(400, "No se pudo crear la categoria correspondiente");
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<CategoriaDTO>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
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
                    var categoriaFind = await _categoriaService.GetOne(id, "CategoriaId");

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
                var categoriaFind = await _categoriaService.GetOne(id, "CategoriaId");

                if (categoriaFind == null)
                {
                    return NotFound("No existe una categoria con ese ID");
                }

                await _categoriaService.Delete(id, "CategoriaId");

                return Ok("Categoria eliminada correctamente!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
