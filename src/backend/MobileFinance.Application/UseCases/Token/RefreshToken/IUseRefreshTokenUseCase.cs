using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Token.RefreshToken;
public interface IUseRefreshTokenUseCase
{
    public Task<ResponseTokensJson> Execute(RequestNewTokenJson request);
}
