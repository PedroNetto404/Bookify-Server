using Bookify.Domain.Tenants.ValueObjects;
using FluentValidation;

namespace Bookify.Application.Tenants.TenantRegister;

public class RegisterTenantCommandValidator : AbstractValidator<RegisterTenantCommand>
{
    public RegisterTenantCommandValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty()
            .NotNull()
            .WithMessage("First name is required.")
            .MaximumLength(FirstName.MaxLength)
            .WithMessage("First name is too long.");

        RuleFor(p => p.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Last name is required.")
            .MaximumLength(LastName.MaxLength)
            .WithMessage("Last name is required.");

        RuleFor(p => p.Email)
            .NotEmpty()
            .NotNull()
            .WithMessage("Email is required.")
            .EmailAddress();
    }
}