using Capa.Datos.Entidades;
using Capa.Datos.Modelos;
using Capa.Infraestructura.Servicios.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly TokenUtilities tokenUtility;

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            _roleManager = roleManager;
            tokenUtility = new TokenUtilities(_configuration);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            ApiResponse<string> response;

            try
            {
                if (!ModelState.IsValid)
                {
                    response = ApiResponse<string>.ErrorResponse(400, ModelState.ToString());
                    return BadRequest(response);
                }

                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var role = await _roleManager.FindByNameAsync("Usuario");
                    var roleAssing = await _userManager.AddToRoleAsync(user, "Usuario");

                    if (roleAssing.Succeeded)
                    {
                        response = ApiResponse<string>.SuccessResponse("Registrado correctamente!", 200);
                        return Ok(response);
                    }
                }

                response = ApiResponse<string>.ErrorResponse(400, result.Errors.FirstOrDefault().Description);
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response = ApiResponse<string>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            ApiResponse<string> response;

            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);

                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var userRole = await _userManager.GetRolesAsync(user);
                    var accessToken = tokenUtility.GenerateJwtToken(user.UserName, user.Id, userRole.FirstOrDefault(), 20);
                    var refreshToken = tokenUtility.GenerateJwtToken(user.UserName, user.Id, userRole.FirstOrDefault(), (7 * 24 * 60));

                    var accessTokenCookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddMinutes(20)
                    };
                    Response.Cookies.Append("accessToken", accessToken, accessTokenCookieOptions);

                    var refreshTokenCookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTime.UtcNow.AddDays(7)
                    };

                    Response.Cookies.Append("refreshToken", refreshToken, refreshTokenCookieOptions);

                    response = ApiResponse<string>.SuccessResponse("Logueado correctamente!", 200);
                    return Ok(response);
                }

                response = ApiResponse<string>.ErrorResponse(400, "Error al intentar loguearse");
                return Unauthorized(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<string>.ErrorResponse(500, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {

            ApiResponse<string> response;

            try
            {
                foreach (var cookie in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookie);

                }

                response = ApiResponse<string>.SuccessResponse("Logout exitoso", 200);
                return Ok(response);

            }
            catch (Exception ex)
            {
                response = ApiResponse<string>.ErrorResponse(400, ex.Message);
                return BadRequest(response);
            }
        }

        [HttpPost("RefreshToken")]
        [Authorize]
        public IActionResult RefreshToken()
        {
            var newAccessToken = tokenUtility.RefreshToken(HttpContext);

            if (newAccessToken == null)
            {
                return Unauthorized(ApiResponse<string>.ErrorResponse(401, "Invalid refresh token"));
            }

            return Ok(ApiResponse<string>.SuccessResponse("Token refreshed successfully", 200));
        }
    }
}
