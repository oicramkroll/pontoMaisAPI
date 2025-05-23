using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Services.Adm;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdmController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAdmService _admService;   

        public AdmController(IConfiguration configuration, IAdmService admService)
        {
            _admService = admService;
            _configuration = configuration;
        }

        /// <summary>
        /// Método para autenticar o usuário administrador e gerar um token JWT.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest login)
        {
            if (login.Username == "admin" && login.Password == "senha123")
            {
                var claims = new[]
                {
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(ClaimTypes.Name, login.Username)
            };

                var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSettings>();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

                var token = new JwtSecurityToken(
                    issuer: jwtSettings.Issuer,
                    audience: jwtSettings.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }

            return Unauthorized("Login inválido");
        }
        /// <summary>
        /// Método para listar administradores.
        /// </summary>
        /// <param name="test"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("ListaAdminstradores")]
        public async Task<IActionResult> ListaAdminstradores([FromQuery] string test)
        {
            var response = await _admService.GetAllAdmsAsync();
            if (response == null)
            {
                return NotFound("Nenhum administrador encontrado");
            }
            return Ok(response);
        }
    }
}
