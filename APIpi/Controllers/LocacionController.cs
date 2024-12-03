using APIpi.Controllers.LocacionTypes; // Importa los tipos de datos espec�ficos del controlador de Locacion
using APIpi.Model; // Importa los modelos de la aplicaci�n
using Microsoft.AspNetCore.Mvc; // Importa el paquete para construir controladores de API
using Microsoft.EntityFrameworkCore; // Importa el paquete para trabajar con Entity Framework Core

namespace APIpi.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("[controller]")] // Define la ruta base del controlador
    public class LocacionController : ControllerBase
    {
        private readonly ILogger<LocacionController> _logger; // Logger para registrar informaci�n y errores
        private readonly AppDbContext contextDB; // Contexto de base de datos

        // Constructor que inicializa el logger y el contexto de base de datos
        public LocacionController(ILogger<LocacionController> logger, AppDbContext context)
        {
            _logger = logger;
            contextDB = context;
        }

        [HttpPost(Name = "PostLocacion")] // Maneja las solicitudes HTTP POST
        public async Task<ActionResult<PostLocacionResponse>> Post(PostLocacionRequest request)
        {
            // Crea una nueva locaci�n a partir de la solicitud
            var locacion = new Locacion
            {
                Nombre_Locacion = request.Nombre_Locacion,
                Tipo_Locacion = request.Tipo_Locacion,
                Capacidad_Maxima = request.Capacidad_Maxima,
                Direcci�n = request.Direcci�n,
                Precio_Base = request.Precio_Base
            };

            // Agrega la nueva locaci�n al contexto de base de datos
            contextDB.Locaciones.Add(locacion);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();

            // Prepara la respuesta
            var response = new PostLocacionResponse
            {
                ID_Locacion = locacion.ID_Locacion,
                Nombre_Locacion = locacion.Nombre_Locacion,
                Tipo_Locacion = locacion.Tipo_Locacion,
                Capacidad_Maxima = locacion.Capacidad_Maxima,
                Direcci�n = locacion.Direcci�n,
                Precio_Base = locacion.Precio_Base
            };

            // Devuelve una respuesta 201 Created con la ubicaci�n del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = locacion.ID_Locacion }, locacion);
        }

        [HttpGet("{id}")] // Maneja las solicitudes HTTP GET con un par�metro id
        public async Task<ActionResult<Locacion>> GetById(int id)
        {
            // Registra informaci�n sobre la b�squeda de la locaci�n
            _logger.LogInformation($"Fetching locacion with id [{id}] from the data base...");
            // Busca la locaci�n en la base de datos por ID
            var locacion = await contextDB.Locaciones.FindAsync(id);
            if (locacion == null)
            {
                // Si no se encuentra la locaci�n, registra una advertencia y devuelve 404 NotFound
                _logger.LogWarning($"Locacion with id [{id}] was not found");
                return NotFound();
            }

            // Registra informaci�n sobre la locaci�n encontrada
            _logger.LogInformation($"Found locacion with id [{id}]");
            // Prepara la respuesta
            var response = new Locacion
            {
                ID_Locacion = locacion.ID_Locacion,
                Nombre_Locacion = locacion.Nombre_Locacion,
                Tipo_Locacion = locacion.Tipo_Locacion,
                Capacidad_Maxima = locacion.Capacidad_Maxima,
                Direcci�n = locacion.Direcci�n,
                Precio_Base = locacion.Precio_Base
            };

            // Devuelve la respuesta 200 OK con la locaci�n encontrada
            return Ok(response);
        }

        [HttpGet(Name = "GetAllLocaciones")] // Maneja las solicitudes HTTP GET para obtener todas las locaciones
        public async Task<ActionResult<IEnumerable<Locacion>>> GetAll()
        {
            // Obtiene todas las locaciones de la base de datos
            return await contextDB.Locaciones.ToListAsync();
        }

        [HttpPut("{id}")] // Maneja las solicitudes HTTP PUT con un par�metro id
        public async Task<ActionResult<PutLocacionResponse>> Put(int id, PutLocacionRequest request)
        {
            // Crea un nuevo objeto Locacion con los datos proporcionados
            var locacionToUpdate = new Locacion
            {
                ID_Locacion = id,
                Nombre_Locacion = request.Nombre_Locacion,
                Tipo_Locacion = request.Tipo_Locacion,
                Capacidad_Maxima = request.Capacidad_Maxima,
                Direcci�n = request.Direcci�n,
                Precio_Base = request.Precio_Base
            };

            // Actualiza la locaci�n en el contexto
            contextDB.Locaciones.Update(locacionToUpdate);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();

            // Devuelve la respuesta 200 OK con la locaci�n actualizada
            return Ok(await contextDB.Locaciones.FindAsync(id));
        }

        [HttpDelete("{id}")] // Maneja las solicitudes HTTP DELETE con un par�metro id
        public async Task<IActionResult> Delete(int id)
        {
            // Busca la locaci�n en la base de datos por ID
            var locacion = await contextDB.Locaciones.FindAsync(id);
            if (locacion == null)
            {
                // Si no se encuentra la locaci�n, devuelve 404 NotFound
                return NotFound();
            }
            // Elimina la locaci�n del contexto
            contextDB.Locaciones.Remove(locacion);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();
            // Devuelve una respuesta 204 No Content
            return NoContent();
        }
    }
}
