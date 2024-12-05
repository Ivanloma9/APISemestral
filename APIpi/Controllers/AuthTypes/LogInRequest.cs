
using System.ComponentModel.DataAnnotations;

namespace APIpi.Controllers.AuthTypes;

public class LogInRequest
{
    [Required]
    [MaxLength(100)]
    public required string Correo_Electrónico { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Contraseña { get; set; }
}