using static MandrilAPI.Models.Habilidad;

namespace MandrilAPI.DTOs;

public class HabilidadCreateDto
{
    public string Nombre { get; set; } = string.Empty;
    public EPotencia Potencia { get; set; }

}
