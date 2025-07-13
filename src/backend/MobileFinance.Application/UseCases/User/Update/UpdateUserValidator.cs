using FluentValidation;
using MobileFinance.Communication.Requests;
using MobileFinance.Exceptions;

namespace MobileFinance.Application.UseCases.User.Update;
public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ExceptionMessages.EMPTY_NAME);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ExceptionMessages.EMPTY_EMAIL);
        When(request => !string.IsNullOrWhiteSpace(request.Name), () => RuleFor(request => request.Name)
        .MinimumLength(3)
        .WithMessage(ExceptionMessages.INVALID_NAME));
        When(request => !string.IsNullOrWhiteSpace(request.Email), () => RuleFor(request => request.Email)
        .EmailAddress()
        .WithMessage(ExceptionMessages.INVALID_EMAIL));
    }
}
