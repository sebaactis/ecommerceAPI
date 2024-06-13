using AutoMapper;
using Capa.Aplicacion.DTO;
using Capa.Aplicacion.Servicios.Interfaces;
using Capa.Datos.Entidades;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdenController : ControllerBase
    {
        private readonly IOrdenService _ordenService;
        private readonly IMapper _mapper;

        public OrdenController(IOrdenService ordenService, IMapper mapper)
        {
            _ordenService = ordenService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrden(int cartId)
        {
            try
            {
                await _ordenService.Create(cartId);

                return Ok("Orden creada correctamente");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<OrdenDTO>> GetOrden(int ordenId)
        {
            try
            {
                var orden = await _ordenService.Get(ordenId);

                if (orden == null)
                {
                    return NotFound("Orden no encontrada bajo ese ID");
                }

                var ordenDto = _mapper.Map<OrdenDTO>(orden);

                return Ok(ordenDto);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
