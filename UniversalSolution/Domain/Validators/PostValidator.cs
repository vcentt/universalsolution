using FluentValidation;
using UniversalSolution.Domain.DTOs;

namespace UniversalSolution.Domain.Validators;

public class PostValidator : AbstractValidator<CreatePostDto>
{
    public PostValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("El título es requerido")
            .MaximumLength(200).WithMessage("El título no puede exceder 200 caracteres");

        RuleFor(x => x.Body)
            .NotEmpty().WithMessage("El contenido es requerido")
            .MaximumLength(1000).WithMessage("El contenido no puede exceder 1000 caracteres");

        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("El UserId debe ser mayor a 0");
    }
}