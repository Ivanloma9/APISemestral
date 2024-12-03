using APIpi.Controllers.DetaServiTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIpi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DetallesServiciosController : ControllerBase
    {
        private readonly ILogger<DetallesServiciosController> _logger; 
        private readonly AppDbContext contextDB; //interactua con las tablas de la bd

        //El contructor hace las dependecias con la base de datos y el logger
        public DetallesServiciosController(ILogger<DetallesServiciosController> logger, AppDbContext context)
        {
            _logger = logger;
            contextDB = context;
        }

        //este endpoint crea una nuva fila en la tabla DetallesServicios
        [HttpPost(Name = "PostDetallesServicios")]
        public async Task<ActionResult<PostDetServiResponse>> Post(PostDetServiRequest request)
        {
            //se crea una entidad a partir de los datos de la solicitud
            var servicio = new DetallesServicios
            {
              Notas_Adicionales = request.Notas_Adicionales,
              ID_Evento = request.ID_Eventos,
              ID_Servicio = request.ID_Servicio
            };
            contextDB.Detalles_Servicios.Add(servicio); //agrega la entidad al contexto
            await contextDB.SaveChangesAsync(); //Guardo los agregado a la BD

            //Se crea una respuesta basada en la entidad creada
            var response = new PostDetServiResponse
            {
                ID_Detalles_Servicios =servicio.ID_Detalles_Servicios,
                Notas_Adicionales = servicio.Notas_Adicionales,
                ID_Eventos = servicio.ID_Evento,
                ID_Servicio = servicio.ID_Servicio
            };
            //retorna el resultado
            return CreatedAtAction(nameof(GetById), new { id = servicio.ID_Detalles_Servicios }, response);
        }

        //Este endpoint trae informacion de la BD segun el ID introducido
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDetServiResponse>> GetById(int id)
        {
            var servicio = await contextDB.Detalles_Servicios.FindAsync(id); //Busca segun el ID
            if (servicio == null) //Si no existe el ID devuelve un 400
            {
                return NotFound();
            }

            //Construye la respuesta con los datos
            var response = new GetDetServiResponse
            {
                ID_Detalles_Servicios = servicio.ID_Detalles_Servicios,
                Notas_Adicionales = servicio.Notas_Adicionales,
                ID_Eventos = servicio.ID_Evento,
                ID_Servicio = servicio.ID_Servicio
            };

            return Ok(response); //Devuelve lo que encontro
        }

        //Endpoint que trae todo la informacion almacenada en la tabla DetallesServicios
        [HttpGet(Name = "GetAllDetallesServicios")]
        public async Task<ActionResult<IEnumerable<DetallesServicios>>> GetAll()
        {
            //consulta la tabla y transforma cada unos de los datos en un modelo de respuesta
            var servicios = await contextDB.Detalles_Servicios.Select(servicio => new GetDetServiResponse
            {
                ID_Detalles_Servicios = servicio.ID_Detalles_Servicios,
                Notas_Adicionales = servicio.Notas_Adicionales,
                ID_Eventos = servicio.ID_Evento,
                ID_Servicio = servicio.ID_Servicio
            }).ToListAsync();

            return Ok(servicios);
        }

        //Endpoint que actualiza segun el id
        [HttpPut("{id}")]
        public async Task<ActionResult<PutDetServiResponse>> Put(int id, PutDetServiRequest request)
        {
            //Crea una nueva entidad con los datos actualizados
            var serviciosToUpdate = new DetallesServicios
            {
                ID_Detalles_Servicios = id, //verifica que el ID coincida
                Notas_Adicionales = request.Notas_Adicionales,
                ID_Evento = request.ID_Eventos,
                ID_Servicio = request.ID_Servicio
            };

            contextDB.Detalles_Servicios.Update(serviciosToUpdate); //Marca la entidad como que se modifico
            await contextDB.SaveChangesAsync(); //Guarda los cambios hechos

            var updatedDetServi = await contextDB.Detalles_Servicios.FindAsync(id);

            var response = new PutDetServiResponse
            {
                ID_Detalles_Servicios = id,
                Notas_Adicionales = updatedDetServi.Notas_Adicionales,
                ID_Eventos = updatedDetServi.ID_Evento,
                ID_Servicio = updatedDetServi.ID_Servicio
            };

            return Ok(response); //devuelve todo los datos + lo actualizado
        }

        //Endpoint que elimina un dato segun el id de este
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var servicio = await contextDB.Detalles_Servicios.FindAsync(id); //Busca el detalle segun el id ingresado
            if (servicio == null) //Si no exite devuelve un 400
            {
                return NotFound();
            }
            contextDB.Detalles_Servicios.Remove(servicio); //Marca el detalle para poder eliminarla
            await contextDB.SaveChangesAsync(); 
            return NoContent(); //Devuelve un 200 confirmando la eliminacion
        }
    }
}
