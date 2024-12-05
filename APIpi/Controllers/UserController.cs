using APIpi.Controllers.UsuarioController;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIpi.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("[controller]")] // Define la ruta del controlador
    public class UserController(ILogger<UserController> logger, AppDbContext context) : ControllerBase
    {
        // Logger para registrar información y errores
        // Contexto de base de datos

        // Constructor que inicializa el logger y el contexto de base de datos

        [HttpPost(Name = "PostUser")] // Maneja las solicitudes HTTP POST
        public async Task<ActionResult<PostUserResponse>> Post(PostUserRequest request)
        {
            // Crea un nuevo usuario a partir de la solicitud
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Correo_Electrónico = request.Correo_Electrónico,
                Contraseña = request.Contraseña,
                Teléfono = request.Teléfono,
                Dirección = request.Dirección,
                Tipo = request.Tipo
            };
            // Agrega el nuevo usuario al contexto
            context.Usuario.Add(usuario);
            // Guarda los cambios en la base de datos
            await context.SaveChangesAsync();

            // Prepara la respuesta
            var response = new PostUserResponse
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = usuario.Tipo
            };

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetByCorreo), new { correoElectronico = usuario.Correo_Electrónico }, response);
        }

        [HttpGet("{correoElectronico}")] // Maneja las solicitudes HTTP GET con un parámetro id
        public async Task<ActionResult<GetUserResponse>> GetByCorreo(string correoElectronico)
        {
            // Registra información sobre la búsqueda del usuario
            logger.LogInformation(
                $"Buscando usuario con correo electronico [{correoElectronico}] en la base de datos...");
            // Busca el usuario en la base de datos por ID
            var usuario = await context.Usuario.FindAsync(correoElectronico);
            if (usuario == null)
            {
                // Si no se encuentra el usuario, registra una advertencia y devuelve 404 NotFound
                logger.LogWarning($"No se econtro un usuario con el correo electronico [{correoElectronico}]");
                return NotFound();
            }

            // Registra información sobre el usuario encontrado
            logger.LogInformation($"Se econtro un usuario con el correo electronico [{correoElectronico}]");
            // Prepara la respuesta
            var response = new GetUserResponse
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = usuario.Tipo
            };

            // Devuelve la respuesta 200 OK con el usuario encontrado
            return Ok(response);
        }

        [HttpGet(Name = "GetAllUsers")] // Maneja las solicitudes HTTP GET para obtener todos los usuarios
        public async Task<ActionResult<IEnumerable<GetUserResponse>>> GetAll()
        {
            // Obtiene todos los usuarios de la base de datos
            var usuarios = await context.Usuario.ToListAsync();
            // Prepara la respuesta
            var response = usuarios.Select(usuario => new GetUserResponse
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = usuario.Tipo
            }).ToList();

            // Devuelve la respuesta 200 OK con la lista de usuarios
            return Ok(response);
        }

        [HttpPut("{correoElectronico}")] // Maneja las solicitudes HTTP PUT con un parámetro id
        public async Task<ActionResult<PutUserResponse>> Put(string correoElectronico, PutUserRequest request)
        {
            // Registra información sobre la búsqueda del usuario
            logger.LogInformation(
                $"Buscando usuario con correo electronico [{correoElectronico}] en la base de datos...");
            // Busca el usuario en la base de datos por ID
            var usuario = await context.Usuario.FindAsync(correoElectronico);
            if (usuario == null)
            {
                // Si no se encuentra el usuario, registra una advertencia y devuelve 404 NotFound
                logger.LogWarning($"No se econtro un usuario con el correo electronico [{correoElectronico}]");
                return NotFound();
            }

            // Crea un nuevo objeto Usuario con los datos proporcionados
            usuario.Nombre = request.Nombre;
            usuario.Apellido = request.Apellido;
            usuario.Correo_Electrónico = request.Correo_Electrónico;
            usuario.Contraseña = request.Contraseña;
            usuario.Teléfono = request.Teléfono;
            usuario.Dirección = request.Dirección;
            usuario.Tipo = request.Tipo;

            // Actualiza el usuario en el contexto
            context.Usuario.Update(usuario);
            // Guarda los cambios en la base de datos
            await context.SaveChangesAsync();

            var response = new PutUserResponse
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = usuario.Tipo
            };

            // Devuelve la respuesta 200 OK con el usuario actualizado
            return Ok(response);
        }

        [HttpDelete("{correoElectronico}")] // Maneja las solicitudes HTTP DELETE con un parámetro id
        public async Task<IActionResult> Delete(string correoElectronico)
        {
            // Busca el usuario en la base de datos por ID
            var usuario = await context.Usuario.FindAsync(correoElectronico);
            if (usuario == null)
            {
                // Si no se encuentra el usuario, devuelve 404 NotFound
                return NotFound();
            }

            // Elimina el usuario del contexto
            context.Usuario.Remove(usuario);
            // Guarda los cambios en la base de datos
            await context.SaveChangesAsync();
            // Devuelve una respuesta 204 No Content
            return NoContent();
        }
    }
}