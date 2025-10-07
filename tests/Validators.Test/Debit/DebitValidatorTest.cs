using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Debit;
using MobileFinance.Communication.Enums;
using MobileFinance.Exceptions;
using Shouldly;

namespace Validators.Test.Debit;
public class DebitValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_UseBusinessDay()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.DebitType = DebitType.OneTime;
        request.UseBusinessDay = null;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_RecurrenceMonthsCount()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.DebitType = DebitType.OneTime;
        request.RecurrenceMonthsCount = null;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Empty_Title()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.Title = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_DEBIT_TITLE));
    }

    [Fact]
    public void Error_Empty_Amount()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.Amount = 0;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_AMOUNT));
    }

    [Fact]
    public void Error_Invalid_Debit_Type()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.DebitType = (DebitType)1000;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.DEBIT_TYPE_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_Invalid_DayOfMonth()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.PaidOn = default;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_PAID_DATE));
    }

    [Fact]
    public void Error_Invalid_UseBusinessDay()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.DebitType = DebitType.Recurring;
        request.UseBusinessDay = null;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.USE_BUSINESS_DAY_NULL));
    }

    [Fact]
    public void Error_Invalid_RecurrenceMonthsCount()
    {
        var validator = new DebitValidator();
        var request = RequestDebitJsonBuilder.Build();
        request.DebitType = DebitType.Recurring;
        request.RecurrenceMonthsCount = null;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        //validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.USE_BUSINESS_DAY_NULL));
    }
}
