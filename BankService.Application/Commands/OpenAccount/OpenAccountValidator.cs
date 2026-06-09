using FluentValidation;

namespace BankService.Application.Commands.OpenAccount;

/// <summary>
/// Input validation for <see cref="OpenAccountCommand"/>. This guards the request
/// shape (presence of data); business eligibility rules remain in the domain.
/// </summary>
public sealed class OpenAccountValidator : AbstractValidator<OpenAccountCommand>
{
    public OpenAccountValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full name is required.")
            .MaximumLength(200);

        RuleFor(x => x.DocumentContent)
            .NotNull().WithMessage("An identity/onboarding document is required.");

        RuleFor(x => x.DocumentFileName)
            .NotEmpty().WithMessage("Document file name is required.");

        RuleFor(x => x.DocumentContentType)
            .NotEmpty().WithMessage("Document content type is required.");
    }
}
