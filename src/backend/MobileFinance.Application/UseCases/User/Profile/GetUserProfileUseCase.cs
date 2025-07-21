using Mapster;
using MobileFinance.Communication.Responses;
using MobileFinance.Domain.Services.LoggedUser;

namespace MobileFinance.Application.UseCases.User.Profile;
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly ILoggedUser _loggedUser;

    public GetUserProfileUseCase(ILoggedUser loggedUser)
    {
        _loggedUser = loggedUser;
    }

    public async Task<ResponseUserProfileJson> Execute()
    {
        var user = await _loggedUser.GetUser();

        return user.Adapt<ResponseUserProfileJson>();
    }
}
