using Capa.Datos.Entidades;
using Capa.Datos.Modelos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("GetAll")]
        public IActionResult Get()
        {
            ApiResponse<IQueryable<IdentityRole>> response;

            try
            {
                var roles = _roleManager.Roles;
                response = ApiResponse<IQueryable<IdentityRole>>.SuccessResponse(roles, 200);
                return Ok(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<IQueryable<IdentityRole>>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpGet("GetOne")]
        public async Task<IActionResult> GetOne([FromBody] RoleModel roleModel)
        {
            ApiResponse<IdentityRole> response;

            try
            {
                var role = await _roleManager.FindByNameAsync(roleModel.Name);

                if (role == null)
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(404, "No existe un rol con ese nombre");
                    return NotFound(response);
                }

                response = ApiResponse<IdentityRole>.SuccessResponse(role, 200);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<IdentityRole>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] RoleModel roleModel)
        {
            ApiResponse<IdentityRole> response;

            try
            {
                if (string.IsNullOrEmpty(roleModel.Name) && string.IsNullOrWhiteSpace(roleModel.Name))
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(400, "El rol no puede estar vacio o tener espacios");
                    return BadRequest(response);
                }

                var roleExist = await _roleManager.RoleExistsAsync(roleModel.Name);

                if (roleExist)
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(400, "El rol ya existe");
                    return BadRequest(response);
                }

                var role = new IdentityRole(roleModel.Name);
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    response = ApiResponse<IdentityRole>.SuccessResponse(role, 201, "Rol creado correctamente!");
                    return Ok(response);
                }

                response = ApiResponse<IdentityRole>.ErrorResponse(400, "Ocurrio un error y no se pudo crear el rol");
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<IdentityRole>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }

        }

        [HttpPut("Editar")]
        public async Task<IActionResult> Update([FromBody] UpdateRoleModel roleModel)
        {
            ApiResponse<IdentityRole> response;

            try
            {
                if (string.IsNullOrEmpty(roleModel.NewName) && string.IsNullOrWhiteSpace(roleModel.NewName) && string.IsNullOrEmpty(roleModel.Name) && string.IsNullOrWhiteSpace(roleModel.Name))
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(400, "El rol no puede estar vacio o tener espacios");
                    return BadRequest(response);
                }

                var roleExist = await _roleManager.FindByNameAsync(roleModel.Name);

                if (roleExist == null)
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(404, "El rol no existe");
                    return NotFound(response);
                }

                roleExist.Name = roleModel.NewName;

                var result = await _roleManager.UpdateAsync(roleExist);

                if (result.Succeeded)
                {
                    response = ApiResponse<IdentityRole>.SuccessResponse(roleExist, 200, "Rol editado correctamente!");
                    return Ok(response);
                }

                response = ApiResponse<IdentityRole>.ErrorResponse(400, "Ocurrio un error y no se pudo editar el rol");
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<IdentityRole>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpDelete("Eliminar")]

        public async Task<IActionResult> Delete([FromBody] RoleModel roleModel)
        {
            ApiResponse<IdentityRole> response;

            try
            {
                var role = await _roleManager.FindByNameAsync(roleModel.Name);

                if (role == null)
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(404, "El rol no existe");
                    return NotFound(response);
                }

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    response = ApiResponse<IdentityRole>.SuccessResponse(role, 200, "Rol eliminado correctamente");
                    return Ok(response);
                }

                response = ApiResponse<IdentityRole>.ErrorResponse(400, "Ocurrió un error al eliminar el rol");
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<IdentityRole>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("AsignarRol")]

        public async Task<IActionResult> Assing([FromBody] AssingRoleModel roleModel)
        {
            ApiResponse<IdentityRole> response;

            try
            {
                var user = await _userManager.FindByNameAsync(roleModel.UserName);

                if (user == null)
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(404, "No existe el usuario indicado");
                    return NotFound(response);
                }

                var existingRole = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, existingRole);

                var role = await _roleManager.FindByNameAsync(roleModel.RoleName);

                if (role == null)
                {
                    response = ApiResponse<IdentityRole>.ErrorResponse(404, "No existe el rol indicado");
                    return NotFound(response);
                }

                var result = await _userManager.AddToRoleAsync(user, roleModel.RoleName);

                if (result.Succeeded)
                {
                    response = ApiResponse<IdentityRole>.SuccessResponse(role, 200, "Se asigno el rol correctamente");
                    return Ok(response);
                }

                response = ApiResponse<IdentityRole>.ErrorResponse(400, "Ocurrió un error al intentar asignar el rol");
                return BadRequest(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<IdentityRole>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }

            

        }
    }
}
