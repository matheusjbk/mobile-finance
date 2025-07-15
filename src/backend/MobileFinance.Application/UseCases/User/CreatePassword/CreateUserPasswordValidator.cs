using FluentValidation;
using MobileFinance.Application.SharedValidators;
using MobileFinance.Communication.Requests;

namespace MobileFinance.Application.UseCases.User.CreatePassword;
public class CreateUserPasswordValidator : AbstractValidator<RequestCreateUserPasswordJson>
{
    public CreateUserPasswordValidator()
    {
        RuleFor(request => request.Password).SetValidator(new PasswordValidator<RequestCreateUserPasswordJson>());
    }
}
