using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Income;
using MobileFinance.Communication.Enums;
using MobileFinance.Exceptions;
using Shouldly;

namespace Validators.Test.Income;
public class IncomeValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_DayOfMonth()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();
        request.IncomeType = IncomeType.OneTime;
        request.DayOfMonth = null;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Success_Without_UseBusinessDay()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();
        request.IncomeType = IncomeType.OneTime;
        request.UseBusinessDay = null;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Empty_Title()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();
        request.Title = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_INCOME_TITLE));
    }

    [Fact]
    public void Error_Empty_Amount()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();
        request.Amount = 0;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_AMOUNT));
    }

    [Fact]
    public void Error_Invalid_Income_Type()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();
        request.IncomeType = (IncomeType)1000;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.INCOME_TYPE_NOT_SUPPORTED));
    }

    [Fact]
    public void Error_Invalid_DayOfMonth()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();
        request.IncomeType = IncomeType.Salary;
        request.DayOfMonth = 0;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.DAY_OUT_OF_RANGE));
    }

    [Fact]
    public void Error_Invalid_UseBusinessDay()
    {
        var validator = new IncomeValidator();
        var request = RequestIncomeJsonBuilder.Build();
        request.IncomeType = IncomeType.Salary;
        request.UseBusinessDay = null;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.USE_BUSINESS_DAY_NULL));
    }
}
