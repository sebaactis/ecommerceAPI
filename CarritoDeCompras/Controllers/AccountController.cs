using Capa.Datos.Entidades;
using Capa.Datos.Modelos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CarritoDeCompras.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
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
                    response = ApiResponse<string>.SuccessResponse("Registrado correctamente!", 200);
                    return Ok(response);
                }

                response = ApiResponse<string>.ErrorResponse(400, "Ocurrio un error al intentar registrarse");
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
                    var token = GenerateJwtToken(user.UserName, user.Id);

                    var cookieClaims = new List<Claim>
                       {
                         new Claim("JWT", token)
                       };

                    var claimsIdentity = new ClaimsIdentity(cookieClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    Response.Cookies.Append("JWT", token);

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

        private string GenerateJwtToken(string username, string id)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("UserId", id)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return tokenHandler.WriteToken(token);
        }
    }
}
