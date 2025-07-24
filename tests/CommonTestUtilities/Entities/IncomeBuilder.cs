using Bogus;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Enums;

namespace CommonTestUtilities.Entities;
public class IncomeBuilder
{
    public static Income Build(User user)
    {
        return new Faker<Income>()
            .RuleFor(income => income.Id, () => 1)
            .RuleFor(income => income.Title, f => f.Lorem.Word())
            .RuleFor(income => income.Amount, f => (long)f.Finance.Amount(max: 10000))
            .RuleFor(income => income.IncomeType, f => f.PickRandom<IncomeType>())
            .RuleFor(income => income.DayOfMonth, f => f.Random.Byte(min: 1, max: 31))
            .RuleFor(income => income.UseBusinessDay, f => f.Random.Bool())
            .RuleFor(income => income.UserId, user.Id);
    }
}
