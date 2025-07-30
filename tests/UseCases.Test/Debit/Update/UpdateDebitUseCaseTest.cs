using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Debit.Update;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Debit.Update;
public class UpdateDebitUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var debit = DebitBuilder.Build(user);
        var request = RequestDebitJsonBuilder.Build();
        var useCase = CreateUseCase(user, debit);

        Func<Task> act = async () => await useCase.Execute(request, debit.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Debit_NotFound()
    {
        (var user, _) = UserBuilder.Build();
        var debit = DebitBuilder.Build(user);
        var request = RequestDebitJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(request, debit.Id);

        var exception = await act.ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.DEBIT_NOT_FOUND);
    }

    [Fact]
    public async Task Error_Empty_Title()
    {
        (var user, _) = UserBuilder.Build();
        var debit = DebitBuilder.Build(user);
        var request = RequestDebitJsonBuilder.Build();
        request.Title = string.Empty;
        var useCase = CreateUseCase(user, debit);

        Func<Task> act = async () => await useCase.Execute(request, debit.Id);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMPTY_DEBIT_TITLE);
    }

    private static UpdateDebitUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, MobileFinance.Domain.Entities.Debit? debit = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryBuilder = new DebitUpdateOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(debit is not null)
            repositoryBuilder.GetById(user, debit);

        return new UpdateDebitUseCase(loggedUser, repositoryBuilder.Build(), unitOfWork);
    }
}
