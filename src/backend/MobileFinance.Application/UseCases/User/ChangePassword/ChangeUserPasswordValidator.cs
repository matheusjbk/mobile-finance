using FluentValidation;
using MobileFinance.Application.SharedValidators;
using MobileFinance.Communication.Requests;

namespace MobileFinance.Application.UseCases.User.ChangePassword;
public class ChangeUserPasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangeUserPasswordValidator()
    {
        RuleFor(request => request.NewPassword).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
