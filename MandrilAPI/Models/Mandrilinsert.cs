using System.ComponentModel.DataAnnotations;

namespace MandrilAPI.Models;

public class Mandrilinsert
{
    [Required]
    [MaxLength(50)]
     public string Nombre { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]

    public string Apellido { get; set; } = string.Empty;
}
