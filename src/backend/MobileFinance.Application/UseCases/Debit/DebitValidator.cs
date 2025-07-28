using FluentValidation;
using MobileFinance.Communication.Enums;
using MobileFinance.Communication.Requests;
using MobileFinance.Domain.Entities;
using MobileFinance.Exceptions;

namespace MobileFinance.Application.UseCases.Debit;
public class DebitValidator : AbstractValidator<RequestDebitJson>
{
    public DebitValidator()
    {
        RuleFor(debit => debit.Title).NotEmpty().WithMessage(ExceptionMessages.EMPTY_DEBIT_TITLE);
        RuleFor(debit => debit.Amount).NotEmpty().WithMessage(ExceptionMessages.EMPTY_AMOUNT);
        RuleFor(debit => debit.DebitType).IsInEnum().WithMessage(ExceptionMessages.DEBIT_TYPE_NOT_SUPPORTED);
        RuleFor(debit => debit.PaidOn).NotEmpty().WithMessage(ExceptionMessages.EMPTY_PAID_DATE);
        When(debit => debit.DebitType.Equals(DebitType.Recurring), () =>
            RuleFor(debit => debit.UseBusinessDay).NotNull().WithMessage(ExceptionMessages.USE_BUSINESS_DAY_NULL));
    }
}
