using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestFilterDebitsJsonBuilder
{
    public static RequestFilterDebitsJson Build() =>
        new Faker<RequestFilterDebitsJson>()
            .RuleFor(request => request.FilterDate, f => f.Date.Future());
}
