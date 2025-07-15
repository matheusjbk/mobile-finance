using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.User.ChangePassword;
using MobileFinance.Exceptions;
using Shouldly;

namespace Validators.Test.User.ChangePassword;
public class ChangeUserPasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangeUserPasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeTrue();
    }

    [Fact]
    public void Error_Empty_New_Password()
    {
        var validator = new ChangeUserPasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

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
    public void Error_Invalid_New_Password(int passwordLength)
    {
        var validator = new ChangeUserPasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build(passwordLength);
        request.NewPassword = string.Empty;

        var validationResult = validator.Validate(request);

        validationResult.IsValid.ShouldBeFalse();
        validationResult.Errors.ShouldHaveSingleItem();
        validationResult.Errors.ShouldContain(e => e.ErrorMessage.Equals(ExceptionMessages.EMPTY_PASSWORD));
    }
}
