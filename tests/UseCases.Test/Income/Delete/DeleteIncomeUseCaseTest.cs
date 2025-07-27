using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using MobileFinance.Application.UseCases.Income.Delete;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Income.Delete;
public class DeleteIncomeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var income = IncomeBuilder.Build(user);
        var useCase = CreateUseCase(user, income);

        Func<Task> act = async () => await useCase.Execute(income.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Income_NotFound()
    {
        (var user, _) = UserBuilder.Build();
        var income = IncomeBuilder.Build(user);
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute(income.Id);

        var exception = await act.ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.INCOME_NOT_FOUND);
    }

    private static DeleteIncomeUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, MobileFinance.Domain.Entities.Income? income = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readOnlyRepositoryBuilder = new IncomeReadOnlyRepositoryBuilder();
        var writeOnlyRepository = IncomeWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(income is not null)
            readOnlyRepositoryBuilder.GetById(user, income);

        return new DeleteIncomeUseCase(
            loggedUser,
            readOnlyRepositoryBuilder.Build(),
            writeOnlyRepository,
            unitOfWork);
    }
}
