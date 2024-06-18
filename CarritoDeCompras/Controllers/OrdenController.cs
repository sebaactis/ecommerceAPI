using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Modelos;
using Capa.Infraestructura.Servicios.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _ordenService;
        private readonly IMapper _mapper;
        private readonly HttpAccesor _httpAccesor;

        public OrdenController(IOrdenService ordenService, IMapper mapper, HttpAccesor httpAccesor)
        {
            _ordenService = ordenService;
            _mapper = mapper;
            _httpAccesor = httpAccesor;
        }

        [HttpPost("AddOrden")]
        [Authorize]
        public async Task<IActionResult> CreateOrden(int cartId)
        {
            ApiResponse<OrdenDTO> response;
            try
            {
                var user = _httpAccesor.getUserIdToken();

                if (user == null)
                {
                    response = ApiResponse<OrdenDTO>.ErrorResponse(404, "Usuario no encontrado");
                    return NotFound(response);
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
        [Authorize]
        public async Task<ActionResult<OrdenDTO>> GetOrden(int ordenId)
        {
            ApiResponse<OrdenDTO> response;

            try
            {
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
        [Authorize]

        public async Task<ActionResult<IEnumerable<OrdenDTO>>> GetOrdenes()
        {

            ApiResponse<IEnumerable<OrdenDTO>> response;

            try
            {
                var user = _httpAccesor.getUserIdToken();

                if (user == null)
                {
                    response = ApiResponse<IEnumerable<OrdenDTO>>.ErrorResponse(404, "Usuario no encontrado");
                    return NotFound(response);
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
