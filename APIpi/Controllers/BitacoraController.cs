using APIpi.Controllers.BitacoraTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APIpi.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("[controller]")] // Define la ruta del controlador
    public class BitacoraController(ILogger<BitacoraController> logger, AppDbContext context) : ControllerBase
    {
        [HttpPost(Name = "PostBitacora")]
        public async Task<ActionResult> Post(PostBitacoraRequest request)
        {
            var bitacora = new Bitacora
            {
                Sesion = request.Sesion,
                Usuario = request.Usuario,
                Mensaje = request.Mensaje,
            };
            context.Bitacora.Add(bitacora);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet(Name = "GetAllBitacoras")]
        public async Task<ActionResult<IEnumerable<Bitacora>>> GetAll()
        {
            return await context.Bitacora.ToListAsync();
        }
    }
}