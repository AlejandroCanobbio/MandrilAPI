using System;

namespace MandrilAPI.DTOs;

public class UserCreateDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Clave { get; set; } = string.Empty;
}
