using APIpi.Controllers.AgendaTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace APIpi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AgendaController : ControllerBase
    {
        private readonly ILogger<AgendaController> _logger;
        private readonly AppDbContext contextDB;

        //El contructor para las dependecias del contexto de bd y el logger
        public AgendaController(ILogger<AgendaController> logger, AppDbContext context)
        {
            _logger = logger;
            contextDB = context;
        }

        //Este endpoint funciona para crear agenda
        [HttpPost(Name = "PostAgenda")]
        public async Task<ActionResult<PostAgendaResponse>> Post(PostAgendaRequest request)
        {
            //crea una entidad de Agenda de los datos solicitados
            var agenda = new Agenda
            {
                ID_Evento = request.ID_Evento,
                Fecha_Reserva = request.Fecha_Reserva,
                Fecha_Confirmación = request.Fecha_Confirmación,
                Estado_Reserva = request.Estado_Reserva
            };
            contextDB.Agendas.Add(agenda); //agrega la entidad 
            await contextDB.SaveChangesAsync(); //guardo los cambios en la base de datos

            var response = new PostAgendaResponse
            {
                ID_Agenda = agenda.ID_Agenda,
                ID_Evento = agenda.ID_Evento,
                Fecha_Reserva = agenda.Fecha_Reserva,
                Fecha_Confirmación = agenda.Fecha_Confirmación,
                Estado_Reserva = agenda.Estado_Reserva
            };
            //Devuelve un resultado
            return CreatedAtAction(nameof(GetById), new { id = agenda.ID_Agenda }, response);
        }

        //Este endpoint obtiene una agenda segun el ID
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAgendaResponse>> GetById(int id)
        {
            var agenda = await contextDB.Agendas.FindAsync(id); //Busca la ageda por ID
            if (agenda == null) //Si no exite la agenda devuelve un error
            {
                return NotFound();
            }

            var response = new GetAgendaResponse
            {
                ID_Agenda = agenda.ID_Agenda,
                ID_Evento = agenda.ID_Evento,
                Fecha_Reserva = agenda.Fecha_Reserva,
                Fecha_Confirmación = agenda.Fecha_Confirmación,
                Estado_Reserva = agenda.Estado_Reserva
            };

            return Ok(response); //Devuelve la agenda encontrada
        }

        //Este endpoint obtiene todas las agendas en la bd
        [HttpGet(Name = "GetAllAgendas")]
        public async Task<ActionResult<IEnumerable<Agenda>>> GetAll()
        {
            //consulta todas las agendas
            var agendas = await contextDB.Agendas.Select(agenda => new GetAgendaResponse
            {
                ID_Agenda = agenda.ID_Agenda,
                ID_Evento = agenda.ID_Evento,
                Fecha_Reserva = agenda.Fecha_Reserva,
                Fecha_Confirmación = agenda.Fecha_Confirmación,
                Estado_Reserva = agenda.Estado_Reserva
            }).ToListAsync();

            return Ok(agendas);
        }

        //Endpoint para actualizar una agenda
        [HttpPut("{id}")]
        public async Task<ActionResult<PutAgendaResponse>> Put(int id, PutAgendaRequest request)
        {
            //Crea una entidad agenda con los datos actualizados
            var agendaToUpdate = new Agenda
            {
                ID_Agenda = id, //asegura que el id coincida
                ID_Evento = request.ID_Evento,
                Fecha_Reserva = request.Fecha_Reserva,
                Fecha_Confirmación = request.Fecha_Confirmación,
                Estado_Reserva = request.Estado_Reserva,
            };

            contextDB.Agendas.Update(agendaToUpdate); //Esto marca que se a actualizado la entidad
            await contextDB.SaveChangesAsync(); //Guarda los cambios

            //recupera la agenda que se acaba de modificar para devolverla como respuesta
            var updatedAgenda = await contextDB.Agendas.FindAsync(id);

            var response = new PutAgendaResponse
            {
                ID_Agenda = updatedAgenda.ID_Agenda,
                ID_Evento = updatedAgenda.ID_Evento,
                Fecha_Reserva = updatedAgenda.Fecha_Reserva,
                Fecha_Confirmación = updatedAgenda.Fecha_Confirmación,
                Estado_Reserva = updatedAgenda.Estado_Reserva,
            };

            return Ok(response);
        }

        //Un endpoint que se encarga de eliminar datos por ID
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var agenda = await contextDB.Agendas.FindAsync(id); //Aqui busca la agenda segun el ID ingresado
            if (agenda == null) //Regresa un error
            {
                return NotFound();
            }
            contextDB.Agendas.Remove(agenda); //Marca la agenda a eliminar
            await contextDB.SaveChangesAsync(); //Aplica los cambios hecho en la BD
            return NoContent(); //Devuelve un 200
        }
    }
}
