﻿using FluentValidation;
using MobileFinance.Communication.Enums;
using MobileFinance.Communication.Requests;
using MobileFinance.Exceptions;

namespace MobileFinance.Application.UseCases.Income;
public class IncomeValidator : AbstractValidator<RequestIncomeJson>
{
    public IncomeValidator()
    {
        RuleFor(income => income.Title).NotEmpty().WithMessage(ExceptionMessages.EMPTY_INCOME_TITLE);
        RuleFor(income => income.Amount).NotEmpty().WithMessage(ExceptionMessages.EMPTY_AMOUNT);
        RuleFor(income => income.IncomeType).IsInEnum().WithMessage(ExceptionMessages.INCOME_TYPE_NOT_SUPPORTED);
        When(income => income.IncomeType.Equals(IncomeType.Salary) || income.IncomeType.Equals(IncomeType.Rent), () =>
            RuleFor(income => income.DayOfMonth).NotNull().InclusiveBetween((byte)1, (byte)31).WithMessage(ExceptionMessages.DAY_OUT_OF_RANGE));
        When(income => income.IncomeType.Equals(IncomeType.Salary) || income.IncomeType.Equals(IncomeType.Rent), () =>
            RuleFor(income => income.UseBusinessDay).NotNull().WithMessage(ExceptionMessages.USE_BUSINESS_DAY_NULL));
    }
}
