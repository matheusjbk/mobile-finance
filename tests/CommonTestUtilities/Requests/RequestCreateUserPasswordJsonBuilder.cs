using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestCreateUserPasswordJsonBuilder
{
    public static RequestCreateUserPasswordJson Build(int passwordLength = 10)
    {
        return new Faker<RequestCreateUserPasswordJson>()
            .RuleFor(request => request.Password, f => f.Internet.Password(passwordLength));
    }
}
