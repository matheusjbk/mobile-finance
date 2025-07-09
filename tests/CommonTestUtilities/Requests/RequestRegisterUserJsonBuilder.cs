using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(request => request.Name, f => f.Person.FirstName)
            .RuleFor(request => request.Email, (f, request) => f.Internet.Email(request.Name))
            .RuleFor(request => request.Password, f => f.Internet.Password(passwordLength));
    }
}
