using APIpi.Controllers.FacturasTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIpi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FacturasController : ControllerBase
    {
        private readonly ILogger<FacturasController> _logger;
        private readonly AppDbContext contextDB; //interactua con las tablas de la bd

        //se hace la dependecias de la BD y el logger
        public FacturasController(ILogger<FacturasController> logger, AppDbContext context)
        {
            _logger = logger; 
            contextDB = context;
        }

        //Endpoint que crea una nueva factura
        [HttpPost(Name = "PostFactura")]
        public async Task<ActionResult<PostFacturasResponse>> Post(PostFacturasRequest request)
        {
            //Crea una entidad Factura a partir de los datos que se solicitan
            var factura = new Facturas
            {
                ID_Evento = request.ID_Evento,
                Fecha_Factura = request.Fecha_Factura,
                Monto_Total = request.Monto_Total,
                Método_Pago = request.Método_Pago,
                Estado_Pago = request.Estado_Pago,
            };
            contextDB.Facturas.Add(factura); //Agrega la entidad al contexto
            await contextDB.SaveChangesAsync(); //Guarda los datos en la BD

            //Crea una respuesta basada en la entidad que se creo
            var response = new PostFacturasResponse
            {
                ID_Factura = factura.ID_Factura,
                ID_Evento = factura.ID_Evento,
                Fecha_Factura = factura.Fecha_Factura,
                Monto_Total = factura.Monto_Total,
                Método_Pago = factura.Método_Pago,
                Estado_Pago = factura.Estado_Pago,
            };
            //Devuelve un resultado CreatedAt con un enlace al metodo GetById
            return CreatedAtAction(nameof(GetById), new { id = factura.ID_Factura }, response);
        }

        //Endpoint que devuelve datos segun el ID ingresado
        [HttpGet("{id}")]
        public async Task<ActionResult<GetFacturasResponse>> GetById(int id)
        {
            //Busca los datos de la factura segun el id
            var factura = await contextDB.Facturas.FindAsync(id);
            if (factura == null) //regresa un 400 si no existe el id
            {
                return NotFound();
            }

            //Construye la respuesta con los datos encontrados en el id
            var response = new GetFacturasResponse
            {
                ID_Factura = factura.ID_Factura,
                ID_Evento = factura.ID_Evento,
                Fecha_Factura = factura.Fecha_Factura,
                Monto_Total = factura.Monto_Total,
                Método_Pago = factura.Método_Pago,
                Estado_Pago = factura.Estado_Pago,
            };

            return Ok(response); //Devuelve la factura encontrada
        }

        //Este endpoint regresa todos los datos adentro de Facturas
        [HttpGet(Name = "GetAllFacturas")]
        public async Task<ActionResult<IEnumerable<Facturas>>> GetAll()
        {
            //Consulta todas las facturas y las transforma en un modelo de respuesta
            var facturas = await contextDB.Facturas.Select(factura => new GetFacturasResponse
            {
                ID_Factura = factura.ID_Factura,
                ID_Evento = factura.ID_Evento,
                Fecha_Factura = factura.Fecha_Factura,
                Monto_Total = factura.Monto_Total,
                Método_Pago = factura.Método_Pago,
                Estado_Pago = factura.Estado_Pago
                    
            }).ToListAsync();

            return Ok(facturas); //devuelve la lista de facturas
        }

        //Endpoint que actualiza la tabla Facturas segun el ID
        [HttpPut("{id}")]
        public async Task<ActionResult<PutFacturasResponse>> Put(int id, PutFacturasRequest request)
        {
            //Crea una entidad Facturas con los datos que se cambiaron/actualizaron
            var facturaToUpdate = new Facturas
            {
                ID_Factura = id,
                ID_Evento = request.ID_Evento,
                Fecha_Factura = request.Fecha_Factura,
                Monto_Total = request.Monto_Total,
                Método_Pago = request.Método_Pago,
                Estado_Pago = request.Estado_Pago,
            };

            contextDB.Facturas.Update(facturaToUpdate); //Marca la entidad como modificada
            await contextDB.SaveChangesAsync(); //Guarda los cambios hechos

            //recupera la factura actualizada para devolverla en la respuesta
            var updatedAgenda = await contextDB.Facturas.FindAsync(id);

            //se crea la factura para poder responder
            var response = new PutFacturasResponse
            {
                ID_Factura = facturaToUpdate.ID_Factura,
                ID_Evento = facturaToUpdate.ID_Evento,
                Fecha_Factura = facturaToUpdate.Fecha_Factura,
                Monto_Total = facturaToUpdate.Monto_Total,
                Método_Pago = facturaToUpdate.Método_Pago,
                Estado_Pago = facturaToUpdate.Estado_Pago
            };

            return Ok(response); //Devuelve la respuesta con la factura actualizada
        }

        //Endpoint que elimina una factura segun el id ingresado al programa
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var factura = await contextDB.Facturas.FindAsync(id);
            if (factura == null) //Si no existe devuelve un 400 
            {
                return NotFound();
            }
            contextDB.Facturas.Remove(factura); //Marca la factura para poder eliminarla
            await contextDB.SaveChangesAsync(); //Aplica la eliminacion a la BD
            return NoContent(); //Devuelve un 200 cuando se a eliminado correctamente
        }
    }
}
