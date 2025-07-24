using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using MobileFinance.Application.UseCases.Income.GetById;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Income.GetById;
public class GetIncomeByIdUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var income = IncomeBuilder.Build(user);
        var useCase = CreateUseCase(user, income);

        var response = await useCase.Execute(income.Id);

        response.ShouldNotBeNull();
        response.Title.ShouldBe(income.Title);
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

    private static GetIncomeByIdUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, MobileFinance.Domain.Entities.Income? income = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var readOnlyRepositoryBuilder = new IncomeReadOnlyRepositoryBuilder();

        if(income is not null)
            readOnlyRepositoryBuilder.GetById(user, income);

        return new GetIncomeByIdUseCase(loggedUser, readOnlyRepositoryBuilder.Build());
    }
}
