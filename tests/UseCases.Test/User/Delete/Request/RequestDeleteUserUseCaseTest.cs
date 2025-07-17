using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.ServiceBus;
using MobileFinance.Application.UseCases.User.Delete.Request;
using Shouldly;

namespace UseCases.Test.User.Delete.Request;
public class RequestDeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        Func<Task> act = async () => await useCase.Execute();

        await act.ShouldNotThrowAsync();

        user.Active.ShouldBeFalse();
    }

    private RequestDeleteUserUseCase CreateUseCase(MobileFinance.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);
        var repository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var deleteUserQueue = DeleteUserQueueBuilder.Build();

        return new RequestDeleteUserUseCase(
            loggedUser,
            repository,
            unitOfWork,
            deleteUserQueue);
    }
}
