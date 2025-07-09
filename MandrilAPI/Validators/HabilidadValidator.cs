using FluentValidation;
using MandrilAPI.DTOs;

namespace MandrilAPI.Validators;

public class HabilidadValidator: AbstractValidator<HabilidadCreateDto>
{
    public HabilidadValidator()
    {
        RuleFor(h => h.Nombre)
            .NotEmpty().WithMessage("El nombre de la habilidad es obligatorio.")
            .Length(2, 50).WithMessage("El nombre de la habilidad debe tener entre 2 y 50 caracteres.");

        RuleFor(h => h.Potencia).IsInEnum()
            .WithMessage("La potencia de la habilidad debe estar entre 0 y 4.");
    }
}
