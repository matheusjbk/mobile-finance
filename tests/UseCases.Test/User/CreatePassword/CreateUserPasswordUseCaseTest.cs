using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.User.CreatePassword;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.User.CreatePassword;
public class CreateUserPasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var request = RequestCreateUserPasswordJsonBuilder.Build();

        Func<Task> act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Empty_Password()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var request = RequestCreateUserPasswordJsonBuilder.Build();
        request.Password = string.Empty;

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMPTY_PASSWORD);
    }

    private static CreateUserPasswordUseCase CreateUseCase(MobileFinance.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var passwordEncrypter = PasswordEncrypterBuilder.Build();

        return new CreateUserPasswordUseCase(
            loggedUser,
            repository,
            unitOfWork,
            passwordEncrypter);
    }
}
