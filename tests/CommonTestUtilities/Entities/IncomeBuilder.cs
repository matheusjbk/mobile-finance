using Bogus;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Enums;

namespace CommonTestUtilities.Entities;
public class IncomeBuilder
{
    public static IList<Income> Collection(User user, uint count = 2)
    {
        var incomes = new List<Income>();

        if(count == 0)
            count = 1;

        var incomeId = 1;

        for(var i = 0; i < count; i++)
        {
            var income = Build(user);
            income.Id = incomeId++;
            incomes.Add(income);
        }

        return incomes;
    }

    public static Income Build(User user)
    {
        return new Faker<Income>()
            .RuleFor(income => income.Id, () => 1)
            .RuleFor(income => income.Title, f => f.Lorem.Word())
            .RuleFor(income => income.Amount, f => (long)f.Finance.Amount(max: 10000))
            .RuleFor(income => income.IncomeType, f => f.PickRandom<IncomeType>())
            .RuleFor(income => income.ReceivedOn, f => f.Date.Soon())
            .RuleFor(income => income.UseBusinessDay, f => f.Random.Bool())
            .RuleFor(income => income.UserId, user.Id);
    }
}
