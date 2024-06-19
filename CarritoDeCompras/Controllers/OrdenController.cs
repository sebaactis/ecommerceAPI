using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Modelos;
using Capa.Infraestructura.Servicios.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _ordenService;
        private readonly IMapper _mapper;

        public OrdenController(IOrdenService ordenService, IMapper mapper)
        {
            _ordenService = ordenService;
            _mapper = mapper;
        }

        [HttpPost("AddOrden")]
        public async Task<IActionResult> CreateOrden(int cartId)
        {
            ApiResponse<OrdenDTO> response;
            try
            {
                var user = HttpContext.GetUserIdFromToken();

                if (user == null)
                {
                    response = ApiResponse<OrdenDTO>.ErrorResponse(401, "Usuario no autenticado");
                    return Unauthorized(response);
                }

                var result = _mapper.Map<OrdenDTO>(await _ordenService.Create(user));
                response = ApiResponse<OrdenDTO>.SuccessResponse(result, 200, "Su orden se ha creado correctamente, muchas gracias");
                return Ok(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<OrdenDTO>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("GetOrden")]
        public async Task<ActionResult<OrdenDTO>> GetOrden(int ordenId)
        {
            ApiResponse<OrdenDTO> response;

            try
            {
                var user = HttpContext.GetUserIdFromToken();

                if (user == null)
                {
                    response = ApiResponse<OrdenDTO>.ErrorResponse(401, "Usuario no autenticado");
                    return Unauthorized(response);
                }

                var orden = await _ordenService.Get(ordenId);

                if (orden == null)
                {
                    response = ApiResponse<OrdenDTO>.ErrorResponse(404, "No se encontró la orden solicitada");
                    return NotFound(response);
                }

                var ordenDto = _mapper.Map<OrdenDTO>(orden);
                response = ApiResponse<OrdenDTO>.SuccessResponse(ordenDto, 200);

                return Ok(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<OrdenDTO>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<OrdenDTO>>> GetOrdenes()
        {

            ApiResponse<IEnumerable<OrdenDTO>> response;

            try
            {
                var user = HttpContext.GetUserIdFromToken();

                if (user == null)
                {
                    response = ApiResponse<IEnumerable<OrdenDTO>>.ErrorResponse(401, "Usuario no autenticado");
                    return Unauthorized(response);
                }

                var result = await _ordenService.GetAllById(user);

                if (result != null)
                {
                    var ordenes = _mapper.Map<IEnumerable<OrdenDTO>>(result);
                    response = ApiResponse<IEnumerable<OrdenDTO>>.SuccessResponse(ordenes, 200);
                    return Ok(response);
                }

                response = ApiResponse<IEnumerable<OrdenDTO>>.ErrorResponse(404, "No tiene ordenes actualmente");
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<IEnumerable<OrdenDTO>>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }

        }
    }
}
