using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.User.Profile;
public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}
