using APIpi.Controllers.ServiciosAdTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIpi.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("[controller]")] // Define la ruta del controlador
    public class ServiciosAdController : ControllerBase
    {
        private readonly ILogger<ServiciosAdController> _logger; // Logger para registrar información y errores
        private readonly AppDbContext contextDB; // Contexto de base de datos

        // Constructor que inicializa el logger y el contexto de base de datos
        public ServiciosAdController(ILogger<ServiciosAdController> logger, AppDbContext context)
        {
            _logger = logger;
            contextDB = context;
        }

        [HttpPost(Name = "PostServicioAd")] // Maneja las solicitudes HTTP POST
        public async Task<ActionResult<PostServiciosAdResponse>> Post(PostServiciosAdRequest request)
        {
            // Crea un nuevo servicio adicional a partir de la solicitud
            var servicio = new ServiciosAdicionales
            {
                Nombre_Servicio = request.Nombre_Servicio,
                Precio_Servicio = request.Precio_Servicio,
                Descripción = request.Descripción,
                Teléfono = request.Teléfono
            };

            // Agrega el nuevo servicio al contexto
            contextDB.Servicios_Adicionales.Add(servicio);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();

            // Prepara la respuesta
            var response = new PostServiciosAdResponse
            {
                ID_Servicio = servicio.ID_Servicio,
                Nombre_Servicio = servicio.Nombre_Servicio,
                Precio_Servicio = servicio.Precio_Servicio,
                Descripción = servicio.Descripción,
                Teléfono = servicio.Teléfono
            };

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetById), new { id = servicio.ID_Servicio }, response);
        }

        [HttpGet("{id}")] // Maneja las solicitudes HTTP GET con un parámetro id
        public async Task<ActionResult<GetServiciosAdResponse>> GetById(int id)
        {
            // Busca el servicio en la base de datos por ID
            var servicio = await contextDB.Servicios_Adicionales.FindAsync(id);

            if (servicio == null)
            {
                // Si no se encuentra el servicio, devuelve 404 NotFound
                return NotFound();
            }

            // Prepara la respuesta
            var response = new GetServiciosAdResponse
            {
                ID_Servicio = servicio.ID_Servicio,
                Nombre_Servicio = servicio.Nombre_Servicio,
                Precio_Servicio = servicio.Precio_Servicio,
                Descripción = servicio.Descripción,
                Teléfono = servicio.Teléfono
            };

            // Devuelve la respuesta 200 OK con el servicio encontrado
            return Ok(response);
        }

        [HttpGet(Name = "GetAllServiciosAd")] // Maneja las solicitudes HTTP GET para obtener todos los servicios adicionales
        public async Task<ActionResult<IEnumerable<GetServiciosAdResponse>>> GetAll()
        {
            // Obtiene todos los servicios adicionales de la base de datos
            var servicios = await contextDB.Servicios_Adicionales.ToListAsync();
            // Prepara la respuesta
            var response = servicios.Select(s => new GetServiciosAdResponse
            {
                ID_Servicio = s.ID_Servicio,
                Nombre_Servicio = s.Nombre_Servicio,
                Precio_Servicio = s.Precio_Servicio,
                Descripción = s.Descripción,
                Teléfono = s.Teléfono
            }).ToList();

            // Devuelve la respuesta 200 OK con la lista de servicios adicionales
            return Ok(response);
        }

        [HttpPut("{id}")] // Maneja las solicitudes HTTP PUT con un parámetro id
        public async Task<ActionResult<PutServiciosAdResponse>> Put(int id, PutServiciosAdRequest request)
        {
            // Crea un nuevo objeto ServiciosAdicionales con los datos proporcionados
            var servicioToUpdate = new ServiciosAdicionales
            {
                ID_Servicio = id,
                Nombre_Servicio = request.Nombre_Servicio,
                Precio_Servicio = request.Precio_Servicio,
                Descripción = request.Descripción,
                Teléfono = request.Teléfono
            };

            // Actualiza el servicio en el contexto
            contextDB.Servicios_Adicionales.Update(servicioToUpdate);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();

            // Prepara la respuesta
            var response = new PutServiciosAdResponse
            {
                ID_Servicio = servicioToUpdate.ID_Servicio,
                Nombre_Servicio = servicioToUpdate.Nombre_Servicio,
                Precio_Servicio = servicioToUpdate.Precio_Servicio,
                Descripción = servicioToUpdate.Descripción,
                Teléfono = servicioToUpdate.Teléfono
            };

            // Devuelve la respuesta 200 OK con el servicio actualizado
            return Ok(response);
        }

        [HttpDelete("{id}")] // Maneja las solicitudes HTTP DELETE con un parámetro id
        public async Task<IActionResult> Delete(int id)
        {
            // Busca el servicio en la base de datos por ID
            var servicio = await contextDB.Servicios_Adicionales.FindAsync(id);
            if (servicio == null)
            {
                // Si no se encuentra el servicio, devuelve 404 NotFound
                return NotFound();
            }
            // Elimina el servicio del contexto
            contextDB.Servicios_Adicionales.Remove(servicio);
            // Guarda los cambios en la base de datos
            await contextDB.SaveChangesAsync();
            // Devuelve una respuesta 204 No Content
            return NoContent();
        }
    }
}
