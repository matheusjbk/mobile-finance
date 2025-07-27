using Bogus;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestFilterIncomesJsonBuilder
{
    public static RequestFilterIncomesJson Build() => 
        new Faker<RequestFilterIncomesJson>()
            .RuleFor(request => request.FilterDate, f => f.Date.Future());
}
