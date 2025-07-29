using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Debit.Register;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Debit.Register;
public class RegisterDebitUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestDebitJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_Empty_Title()
    {
        (var user, _) = UserBuilder.Build();
        var request = RequestDebitJsonBuilder.Build();
        request.Title = string.Empty;
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMPTY_DEBIT_TITLE);
    }

    private static RegisterDebitUseCase CreateUseCase(MobileFinance.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = DebitWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new RegisterDebitUseCase(loggedUser, repository, unitOfWork);
    }
}
