using APIpi.Controllers.AuthTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;

namespace APIpi.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("[controller]")] // Define la ruta del controlador
    public class AuthController(ILogger<AuthController> logger, AppDbContext context) : ControllerBase
    {
        [HttpPost("LogIn")]
        public async Task<ActionResult<LogInResponse>> LogIn(LogInRequest request)
        {
            var correoElectrónico = request.Correo_Electrónico;
            var contraseña = request.Contraseña;
            // Registra información sobre la búsqueda del usuario
            logger.LogInformation(
                $"Buscando usuario con correo electrónico [{correoElectrónico}] en la base de datos...");
            // Busca el usuario en la base de datos por ID
            var usuario = await context.Usuario.FindAsync(correoElectrónico);
            if (usuario == null)
            {
                var errorMessage = $"No se encontró un usuario con el correo electrónico [{correoElectrónico}]";
                // Si no se encuentra el usuario, registra una advertencia y devuelve 404 NotFound
                logger.LogError(errorMessage);
                return NotFound(errorMessage);
            }

            if (usuario.Contraseña != contraseña)
            {
                var errorMessage = "La contraseña recibida es incorrecta";
                logger.LogError(errorMessage);
                return Unauthorized(errorMessage);
            }

            var response = new LogInResponse
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = TipoDeUsuario.Cliente
            };

            return Ok(response);
        }
    }
}