using Bogus;
using MobileFinance.Communication.Enums;
using MobileFinance.Communication.Requests;

namespace CommonTestUtilities.Requests;
public class RequestIncomeJsonBuilder
{
    public static RequestIncomeJson Build()
    {
        return new Faker<RequestIncomeJson>()
            .RuleFor(request => request.Title, f => f.Lorem.Word())
            .RuleFor(request => request.Amount, f => (long)f.Finance.Amount(max: 10000))
            .RuleFor(request => request.IncomeType, f => f.PickRandom<IncomeType>())
            .RuleFor(request => request.DayOfMonth, f => f.Random.Byte(min: 1, max: 31))
            .RuleFor(request => request.UseBusinessDay, f => f.Random.Bool());
    }
}
