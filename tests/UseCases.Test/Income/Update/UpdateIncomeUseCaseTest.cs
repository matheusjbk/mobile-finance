using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Income.Update;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Income.Update;
public class UpdateIncomeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var income = IncomeBuilder.Build(user);
        var useCase = CreateUseCase(user, income);
        var request = RequestIncomeJsonBuilder.Build();

        Func<Task> act = async () => await useCase.Execute(request, income.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task Error_Income_NotFound()
    {
        (var user, _) = UserBuilder.Build();
        var income = IncomeBuilder.Build(user);
        var useCase = CreateUseCase(user);
        var request = RequestIncomeJsonBuilder.Build();

        Func<Task> act = async () => await useCase.Execute(request, income.Id);

        var exception = await act.ShouldThrowAsync<NotFoundException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.INCOME_NOT_FOUND);
    }

    [Fact]
    public async Task Error_Empty_Title()
    {
        (var user, _) = UserBuilder.Build();
        var income = IncomeBuilder.Build(user);
        var useCase = CreateUseCase(user, income);
        var request = RequestIncomeJsonBuilder.Build();
        request.Title = string.Empty;

        Func<Task> act = async () => await useCase.Execute(request, income.Id);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMPTY_INCOME_TITLE);
    }

    private static UpdateIncomeUseCase CreateUseCase(MobileFinance.Domain.Entities.User user, MobileFinance.Domain.Entities.Income? income = null)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repositoryBuilder = new IncomeUpdateOnlyRepositoryBuilder();
        var unitOfWork = UnitOfWorkBuilder.Build();

        if(income is not null)
            repositoryBuilder.GetById(user, income);

        return new UpdateIncomeUseCase(loggedUser, repositoryBuilder.Build(), unitOfWork);
    }
}
