using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestRegisterUserJsonBuilder
{
    public static RequestRegisterUserJson Build(int passwordLength = 10, int nameLength = 3)
    {
        return new Faker<RequestRegisterUserJson>()
            .RuleFor(request => request.Name, () => 
            {
                var name = new Faker().Person.FirstName;
                while(name.Length < nameLength)
                    name = new Faker().Person.FirstName;
                return name;
            })
            .RuleFor(request => request.Email, (f, request) => f.Internet.Email(request.Name))
            .RuleFor(request => request.Password, f => f.Internet.Password(passwordLength));
    }
}
