using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build(int nameLength = 3)
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, () => 
            {
                var name = new Faker().Person.FirstName;
                while(name.Length < nameLength)
                    name = new Faker().Person.FirstName;
                return name;
            })
            .RuleFor(request => request.Email, (f, request) => f.Internet.Email(request.Name));
    }
}
