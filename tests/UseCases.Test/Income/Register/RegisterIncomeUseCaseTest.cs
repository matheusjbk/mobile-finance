using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using MobileFinance.Application.UseCases.Income.Register;
using MobileFinance.Exceptions;
using MobileFinance.Exceptions.ExceptionsBase;
using Shouldly;

namespace UseCases.Test.Income.Register;
public class RegisterIncomeUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var request = RequestIncomeJsonBuilder.Build();

        var response = await useCase.Execute(request);

        response.ShouldNotBeNull();
        response.Title.ShouldBe(request.Title);
    }

    [Fact]
    public async Task Error_Empty_Title()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);
        var request = RequestIncomeJsonBuilder.Build();
        request.Title = string.Empty;

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.ShouldThrowAsync<ErrorOnValidationException>();

        exception.GetErrorMessages().ShouldHaveSingleItem()
            .ShouldBe(ExceptionMessages.EMPTY_INCOME_TITLE);
    }

    private RegisterIncomeUseCase CreateUseCase(MobileFinance.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = IncomeWriteOnlyRepositoryBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new RegisterIncomeUseCase(loggedUser, repository, unitOfWork);
    }
}
