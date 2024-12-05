using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using APIpi.Model;

namespace APIpi.Controllers.AuthTypes;

public class LogInResponse
{
    [Required]
    [MaxLength(50)]
    public string Nombre { get; set; }

    [Required]
    [MaxLength(50)]
    public string Apellido { get; set; }
        
    [Key]
    [Required]
    [MaxLength(100)]
    public string Correo_Electrónico { get; set; }

    [MaxLength(255)]
    public string Teléfono { get; set; }

    [MaxLength(255)]
    public string Dirección { get; set; }

    [Required]
    [Column(TypeName = "nvarchar(50)")]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TipoDeUsuario Tipo { get; set; }
}