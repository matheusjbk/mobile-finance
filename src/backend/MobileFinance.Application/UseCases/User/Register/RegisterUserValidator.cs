﻿using FluentValidation;
using MobileFinance.Application.SharedValidators;
using MobileFinance.Communication.Requests;
using MobileFinance.Exceptions;

namespace MobileFinance.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(request => request.Name).NotEmpty().WithMessage(ExceptionMessages.EMPTY_NAME);
        RuleFor(request => request.Email).NotEmpty().WithMessage(ExceptionMessages.EMPTY_EMAIL);
        RuleFor(request => request.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
        When(request => !string.IsNullOrWhiteSpace(request.Name), () => RuleFor(request => request.Name)
        .MinimumLength(3)
        .WithMessage(ExceptionMessages.INVALID_NAME));
        When(request => !string.IsNullOrWhiteSpace(request.Email), () => RuleFor(request => request.Email)
        .EmailAddress()
        .WithMessage(ExceptionMessages.INVALID_EMAIL));
    }
}
