using CommonTestUtilities.Entities;
using CommonTestUtilities.LoggedUser;
using MobileFinance.Application.UseCases.User.Profile;
using Shouldly;

namespace UseCases.Test.User.Profile;
public class GetUserProfileUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        (var user, _) = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var response = await useCase.Execute();

        response.ShouldNotBeNull();
        response.Name.ShouldBe(user.Name);
        response.Email.ShouldBe(user.Email);
    }

    private static GetUserProfileUseCase CreateUseCase(MobileFinance.Domain.Entities.User user)
    {
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetUserProfileUseCase(loggedUser);
    }
}
