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
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                    response = ApiResponse<CategoriaDTO>.ErrorResponse(400, error);
                    return BadRequest(response);
                }

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

        [HttpPut("Editar")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] CategoriaDTI categoriaDTI)
        {
            ApiResponse<CategoriaDTO> response;

            try
            {
                if (!ModelState.IsValid)
                {
                    var error = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).FirstOrDefault();
                    response = ApiResponse<CategoriaDTO>.ErrorResponse(400, error);
                    return BadRequest(response);
                }

                var categoriaFind = await _categoriaService.GetOne(id, "CategoriaId");

                if (categoriaFind == null)
                {
                    response = ApiResponse<CategoriaDTO>.ErrorResponse(404, "No se encuentra una categoria con ese ID");
                    return NotFound(response);
                }

                var categoria = _mapper.Map<Categoria>(categoriaDTI);
                var result = await _categoriaService.Edit(id, categoria);

                if (result != null)
                {
                    var categoriaDTO = _mapper.Map<CategoriaDTO>(result);
                    response = ApiResponse<CategoriaDTO>.SuccessResponse(categoriaDTO, 200, "Se modifico la categoria correctamente");
                    return Ok(response);
                }

                response = ApiResponse<CategoriaDTO>.ErrorResponse(400, "No se pudo editar la categoria");
                return BadRequest(response);
            }

            catch (Exception ex)
            {
                response = ApiResponse<CategoriaDTO>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpDelete("Eliminar")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            ApiResponse<CategoriaDTO> response;

            try
            {
                var categoriaFind = await _categoriaService.GetOne(id, "CategoriaId");

                if (categoriaFind == null)
                {
                    response = ApiResponse<CategoriaDTO>.ErrorResponse(404, "No se encuentra una categoria con ese ID");
                    return NotFound(response);
                }

                var result = await _categoriaService.Delete(id, "CategoriaId");

                if (result != null)
                {
                    var categoriaDTO = _mapper.Map<CategoriaDTO>(result);
                    response = ApiResponse<CategoriaDTO>.SuccessResponse(categoriaDTO, 200, "Se elimino la categoria correctamente");
                    return Ok(response);
                }

                response = ApiResponse<CategoriaDTO>.ErrorResponse(400, "No se pudo eliminar la categoria");
                return BadRequest(response);


            }
            catch (Exception ex)
            {
                response = ApiResponse<CategoriaDTO>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }
    }
}
