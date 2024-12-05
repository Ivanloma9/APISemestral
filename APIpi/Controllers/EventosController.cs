using APIpi.Controllers.EventosTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIpi.Controllers
{
    [ApiController]
    [Route("[controller]")]
// _bdContext Objeto que interactua con la BD
// se hace la dependecias de la BD y el logger
    public class EventosController(ILogger<EventosController> _logger, AppDbContext _bdContext) : ControllerBase
    {
        //Un endpoint que crea un nuevo evento
        [HttpPost(Name = "PostEventos")]
        public async Task<ActionResult<PostEventosResponse>> Post(PostEventosRequest request)
        {
            //Se crea una entidad "eventos" de los datos de la solicitud
            var evento = new Eventos
            {
                Tipo_Evento = request.Tipo_Evento,
                Fecha_Evento = request.Fecha_Evento,
                Hora_Evento = request.Hora_Evento,
                Número_Personas = request.Número_Personas,
                Correo_Electrónico = request.Correo_Electrónico,
                ID_Locacion = request.ID_Locacion
            };
            _bdContext.Eventos.Add(evento); //agrega la entidad 
            await _bdContext.SaveChangesAsync(); //Guarda lo que se agrego a la BD

            //Crea la respuesta basada en la informacion ingresada
            var response = new PostEventosResponse
            {
                ID_Evento = evento.ID_Evento,
                Tipo_Evento = evento.Tipo_Evento,
                Fecha_Evento = evento.Fecha_Evento,
                Hora_Evento = evento.Hora_Evento,
                Número_Personas = evento.Número_Personas,
                Correo_Electrónico = evento.Correo_Electrónico,
                ID_Locacion = evento.ID_Locacion
            };
            return CreatedAtAction(nameof(GetById), new { id = evento.ID_Evento }, response);
        }

        //Endpoint que hace un consulto segun un id ingresado y trae un dato
        [HttpGet("{id}")]
        public async Task<ActionResult<GetEventoResponse>> GetById(int id)
        {
            var eventos = await _bdContext.Eventos.FindAsync(id); //Busca el evento segun el id
            if (eventos == null) //Si no existe devuelve un 400
            {
                return NotFound();
            }

            //Crea la respuesta con los datos segun el ID
            var response = new GetEventoResponse
            {
                ID_Evento = eventos.ID_Evento,
                Tipo_Evento = eventos.Tipo_Evento,
                Fecha_Evento = eventos.Fecha_Evento,
                Hora_Evento = eventos.Hora_Evento,
                Número_Personas = eventos.Número_Personas,
                Correo_Electrónico = eventos.Correo_Electrónico,
                ID_Locacion = eventos.ID_Locacion
            };

            return Ok(response); //Devuelve el evento encontrado
        }

        //Endpoint para obtener todos los eventos almacenados en la BD
        [HttpGet(Name = "GetAllEventos")]
        public async Task<ActionResult<IEnumerable<Eventos>>> GetAll()
        {
            //Consulta los eventos y los transfora a un modelo de respuesta
            var eventos = await _bdContext.Eventos.Select(evento => new GetEventoResponse
            {
                ID_Evento = evento.ID_Evento,
                Tipo_Evento = evento.Tipo_Evento,
                Fecha_Evento = evento.Fecha_Evento,
                Hora_Evento = evento.Hora_Evento,
                Número_Personas = evento.Número_Personas,
                Correo_Electrónico = evento.Correo_Electrónico,
                ID_Locacion = evento.ID_Locacion
            }).ToListAsync();

            return Ok(eventos); //Devuelve la lista de eventos
        }

        //Endpoint que modifica los datos de Evento
        [HttpPut("{id}")]
        public async Task<ActionResult<PutEventosResponse>> Put(int id, PutEventosRequest request)
        {
            //Crea una nueva entidad Eventos que se encarga de almacenar los datos actualizados
            var eventoToUpdate = new Eventos
            {
                ID_Evento = id, //Se asegura qu el ID coincida
                Tipo_Evento = request.Tipo_Evento,
                Fecha_Evento = request.Fecha_Evento,
                Hora_Evento = request.Hora_Evento,
                Número_Personas = request.Número_Personas,
                Correo_Electrónico = request.Correo_Electrónico,
                ID_Locacion = request.ID_Locacion,
            };

            _bdContext.Eventos.Update(eventoToUpdate); //Marca la entidad a cambiar
            await _bdContext.SaveChangesAsync(); //Guarda los cambios hechos

            var updatedEvento = await _bdContext.Eventos.FindAsync(id);

            var response = new PutEventosResponse
            {
                ID_Evento = updatedEvento.ID_Evento,
                Tipo_Evento = updatedEvento.Tipo_Evento,
                Fecha_Evento = updatedEvento.Fecha_Evento,
                Hora_Evento = updatedEvento.Hora_Evento,
                Número_Personas = updatedEvento.Número_Personas,
                Correo_Electrónico = updatedEvento.Correo_Electrónico,
                ID_Locacion = updatedEvento.ID_Locacion,
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var evento = await _bdContext.Eventos.FindAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            _bdContext.Eventos.Remove(evento);
            await _bdContext.SaveChangesAsync();
            return NoContent();
        }
    }
}