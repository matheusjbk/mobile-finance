using MobileFinance.Communication.Requests;

namespace MobileFinance.Application.UseCases.User.ChangePassword;
public interface IChangeUserPasswordUseCase
{
    public Task Execute(RequestChangePasswordJson request);
}
