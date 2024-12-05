﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIpi.Model;

public class Bitacora
{
    [Required]
    [MaxLength(36)]
    public string Sesion { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string Usuario { get; set; }
        
    [Required]
    [Column(TypeName = "nvarchar(max)")]
    public string Mensaje { get; set; }
}