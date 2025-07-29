using Bogus;
using MobileFinance.Communication.Enums;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestDebitJsonBuilder
{
    public static RequestDebitJson Build()
    {
        return new Faker<RequestDebitJson>()
            .RuleFor(request => request.Title, f => f.Lorem.Word())
            .RuleFor(request => request.Amount, f => (long)f.Finance.Amount(max: 10000))
            .RuleFor(request => request.DebitType, f => f.PickRandom<DebitType>())
            .RuleFor(request => request.PaidOn, f => f.Date.Soon())
            .RuleFor(request => request.UseBusinessDay, f => f.Random.Bool());
    }
}
