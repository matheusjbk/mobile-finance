using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using MobileFinance.Application.UseCases.Debit.Delete;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Debit.Delete;
public class DeleteDebitUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var debit = DebitBuilder.Build(user);
        var useCase = CreateUseCase(user, debit);

        Func<Task> act = async () => await useCase.Execute(debit.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Debit_NotFound()
    {
        (var user, _) = UserBuilder.Build();
        var debit = DebitBuilder.Build(user);
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(debit.Id);

        var exception = await act.ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.DEBIT_NOT_FOUND);
    }

    private static DeleteDebitUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, MobileFinance.Domain.Entities.Debit? debit = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readOnlyRepositoryBuilder = new DebitReadOnlyRepositoryBuilder();
        var writeOnlyRepository = DebitWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(debit is not null)
            readOnlyRepositoryBuilder.GetById(user, debit);



        return new DeleteDebitUseCase(loggedUser, readOnlyRepositoryBuilder.Build(), writeOnlyRepository, unitOfWork);
    }
}
