using FluentValidation;
using MandrilAPI.DTOs;


namespace MandrilAPI.Validators;

public class MandrilValidator: AbstractValidator<MandrilCreateDto>
{
    public MandrilValidator()
    {
        RuleFor(m => m.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio.")
            .Length(2, 50).WithMessage("El nombre debe tener entre 2 y 50 caracteres.");

        RuleFor(m => m.Apellido)
            .NotEmpty().WithMessage("El apellido es obligatorio.")
            .Length(2, 50).WithMessage("El apellido debe tener entre 2 y 50 caracteres.");
    }
}
