using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.User.Register;
using MobileFinance.Exceptions;
using Shouldly;

namespace Validators.Test.User.Register;
public class RegisterUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Empty_Name()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMPTY_NAME));
    }

    [Fact]
    public void Error_Invalid_Name()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = "hi";

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_NAME));
    }

    [Fact]
    public void Error_Empty_Email()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMPTY_EMAIL));
    }

    [Fact]
    public void Error_Invalid_Email()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = request.Name;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_EMAIL));
    }

    [Fact]
    public void Error_Empty_Password()
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.EMPTY_PASSWORD));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    [InlineData(6)]
    public void Error_Invalid_Password(int passwordLength)
    {
        var validator = new RegisterUserValidator();
        var request = RequestRegisterUserJsonBuilder.Build(passwordLength);

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceMessagesException.INVALID_PASSWORD));
    }
}
