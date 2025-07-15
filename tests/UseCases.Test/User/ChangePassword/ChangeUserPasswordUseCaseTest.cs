using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.User.ChangePassword;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.ChangePassword;
public class ChangeUserPasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Different_Current_Password()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.DIFFERENT_CURRENT_PASSWORD);
    }

    [Fact]
    public async Task Error_Empty_New_Password()
    {
        (var user, var password) = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.CurrentPassword = password;
        request.NewPassword = string.Empty;
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMPTY_PASSWORD);
    }

    private static ChangeUserPasswordUseCase CreateUseCase(MobileFinance.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new ChangeUserPasswordUseCase(
            loggedUser,
            repository,
            passwordEncrypter,
            unitOfWork);
    }
}
