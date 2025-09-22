using UniversalSolution.Domain.DTOs;

namespace UniversalSolution.Domain.Validators;

using FluentValidation;
using Microsoft.Extensions.Configuration;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator(IConfiguration configuration)
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        var emailRegex = configuration["Regex:Email"];
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .Matches(emailRegex).WithMessage("Email format is invalid.");

        var pwdRegex = configuration["Regex:Password"];
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .Matches(pwdRegex).WithMessage("Password must be min 8 chars, contain upper, lower, digit and symbol.");
    }
}