using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.User.CreatePassword;
using MobileFinance.Exceptions;
using Shouldly;

namespace Validators.Test.User.CreatePassword;
public class CreateUserPasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var request = RequestCreateUserPasswordJsonBuilder.Build();
        var validator = new CreateUserPasswordValidator();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Empty_Password()
    {
        var request = RequestCreateUserPasswordJsonBuilder.Build();
        request.Password = string.Empty;
        var validator = new CreateUserPasswordValidator();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_PASSWORD));
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
        var request = RequestCreateUserPasswordJsonBuilder.Build(passwordLength);
        var validator = new CreateUserPasswordValidator();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.INVALID_PASSWORD));
    }
}
