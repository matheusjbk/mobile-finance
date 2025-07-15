using MobileFinance.Communication.Requests;

namespace MobileFinance.Application.UseCases.User.CreatePassword;
public interface ICreateUserPasswordUseCase
{
    public Task Execute(RequestCreateUserPasswordJson request);
}
