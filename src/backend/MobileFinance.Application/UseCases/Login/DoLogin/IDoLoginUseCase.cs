using MobileFinance.Communication.Requests;
using MobileFinance.Communication.Responses;

namespace MobileFinance.Application.UseCases.Login.DoLogin;
public interface IDoLoginUseCase
{
    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
