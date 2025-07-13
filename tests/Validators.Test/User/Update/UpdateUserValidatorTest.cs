using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.User.Update;
using MobileFinance.Exceptions;
using Shouldly;

namespace Validators.Test.User.Update;
public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Empty_Name()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_NAME));
    }

    [Fact]
    public void Error_Invalid_Name()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = "hi";

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.INVALID_NAME));
    }

    [Fact]
    public void Error_Empty_Email()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_EMAIL));
    }

    [Fact]
    public void Error_Invalid_Email()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = request.Name;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.INVALID_EMAIL));
    }
}
