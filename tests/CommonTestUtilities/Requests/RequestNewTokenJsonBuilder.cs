using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestNewTokenJsonBuilder
{
    public static RequestNewTokenJson Build()
    {
        return new Faker<RequestNewTokenJson>()
            .RuleFor(request => request.RefreshToken, () => Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
    }
}
