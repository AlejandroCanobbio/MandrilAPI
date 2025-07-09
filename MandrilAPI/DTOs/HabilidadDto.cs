using System;

namespace MandrilAPI.DTOs;

public class HabilidadDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public string Potencia { get; set; } = string.Empty;
}
