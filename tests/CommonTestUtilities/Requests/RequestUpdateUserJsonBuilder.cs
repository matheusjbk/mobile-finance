using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build(int nameLength = 2)
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(request => request.Name, f => 
            {
                var name = f.Person.FirstName;
                while(name.Length <= nameLength)
                    name = f.Person.FirstName;
                return name;
            })
            .RuleFor(request => request.Email, (f, request) => f.Internet.Email(request.Name));
    }
}
