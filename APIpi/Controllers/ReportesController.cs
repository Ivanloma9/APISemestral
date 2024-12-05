using APIpi.Controllers.AuthTypes;
using APIpi.Model;
using Microsoft.AspNetCore.Mvc;

namespace APIpi.Controllers
{
    [ApiController] // Indica que esta clase es un controlador de API
    [Route("[controller]")] // Define la ruta del controlador
    public class ReportesController(ILogger<ReportesController> logger, AppDbContext context) : ControllerBase
    {
        // top servicios adicionales
        // top locaciones
        // ingresos mensuales
        // proximos eventos
        // bitacora
    }
}