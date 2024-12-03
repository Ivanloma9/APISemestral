using APIpi.Controllers.UsuarioController;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIpi.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("[controller]")] // Define la ruta del controlador
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger; // Logger para registrar información y errores
        private readonly AppDbContext contextDB; // Contexto de base de datos

        // Constructor que inicializa el logger y el contexto de base de datos
        public UserController(ILogger<UserController> logger, AppDbContext context)
        {
            _logger = logger;
            contextDB = context;
        }

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
            contextDB.Usuario.Add(usuario);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();

            // Prepara la respuesta
            var response = new PostUserResponse
            {
                ID_Usuario = usuario.ID_Usuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Contraseña = usuario.Contraseña,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = usuario.Tipo
            };

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = usuario.ID_Usuario }, usuario);
        }

        [HttpGet("{id}")] // Maneja las solicitudes HTTP GET con un parámetro id
        public async Task<ActionResult<Usuario>> GetById(int id)
        {
            // Registra información sobre la búsqueda del usuario
            _logger.LogInformation($"Fetching user with id [{id}] from the data base...");
            // Busca el usuario en la base de datos por ID
            var usuario = await contextDB.Usuario.FindAsync(id);
            if (usuario == null)
            {
                // Si no se encuentra el usuario, registra una advertencia y devuelve 404 NotFound
                _logger.LogWarning($"User with id [{id}] was not found");
                return NotFound();
            }

            // Registra información sobre el usuario encontrado
            _logger.LogInformation($"Found user with id [{id}]");
            // Prepara la respuesta
            var response = new Usuario
            {
                ID_Usuario = usuario.ID_Usuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Contraseña = usuario.Contraseña,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = usuario.Tipo
            };

            // Devuelve la respuesta 200 OK con el usuario encontrado
            return Ok(response);
        }

        [HttpGet(Name = "GetAllUsers")] // Maneja las solicitudes HTTP GET para obtener todos los usuarios
        public async Task<ActionResult<IEnumerable<Usuario>>> GetAll()
        {
            // Obtiene todos los usuarios de la base de datos
            var usuarios = await contextDB.Usuario.ToListAsync();
            // Prepara la respuesta
            var response = usuarios.Select(usuario => new GetUserResponse
            {
                ID_Usuario = usuario.ID_Usuario,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Correo_Electrónico = usuario.Correo_Electrónico,
                Contraseña = usuario.Contraseña,
                Teléfono = usuario.Teléfono,
                Dirección = usuario.Dirección,
                Tipo = usuario.Tipo
            }).ToList();

            // Devuelve la respuesta 200 OK con la lista de usuarios
            return Ok(response);
        }

        [HttpPut("{id}")] // Maneja las solicitudes HTTP PUT con un parámetro id
        public async Task<ActionResult<PutUserResponse>> Put(int id, PutUserRequest request)
        {
            // Crea un nuevo objeto Usuario con los datos proporcionados
            var usuarioToUpdate = new Usuario
            {
                ID_Usuario = id,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Correo_Electrónico = request.Correo_Electrónico,
                Contraseña = request.Contraseña,
                Teléfono = request.Teléfono,
                Dirección = request.Dirección,
                Tipo = request.Tipo
            };

            // Actualiza el usuario en el contexto
            contextDB.Usuario.Update(usuarioToUpdate);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();

            // Devuelve la respuesta 200 OK con el usuario actualizado
            return Ok(await contextDB.Usuario.FindAsync(id));
        }

        [HttpDelete("{id}")] // Maneja las solicitudes HTTP DELETE con un parámetro id
        public async Task<IActionResult> Delete(int id)
        {
            // Busca el usuario en la base de datos por ID
            var usuario = await contextDB.Usuario.FindAsync(id);
            if (usuario == null)
            {
                // Si no se encuentra el usuario, devuelve 404 NotFound
                return NotFound();
            }
            // Elimina el usuario del contexto
            contextDB.Usuario.Remove(usuario);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();
            // Devuelve una respuesta 204 No Content
            return NoContent();
        }
    }
}
