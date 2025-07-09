namespace MandrilAPI.DTOs;

public class MandrilResponseDto
{
    public string Id { get; set; } = null!;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public List<HabilidadDto>? Habilidades { get; set; } = new List<HabilidadDto>();
}