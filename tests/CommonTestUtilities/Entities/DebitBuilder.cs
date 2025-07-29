using Bogus;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Enums;

namespace CommonTestUtilities.Entities;
public class DebitBuilder
{
    public static IList<Debit> Collection(User user, uint count = 2)
    {
        var debits = new List<Debit>();

        if (count == 0)
            count = 1;

        var debitId = 1;

        for(var i = 0; i < count; i++)
        {
            var debit = Build(user);
            debit.Id = debitId++;
            debits.Add(debit);
        }

        return debits;
    }

    public static Debit Build(User user)
    {
        return new Faker<Debit>()
            .RuleFor(debit => debit.Id, () => 1)
            .RuleFor(debit => debit.Title, f => f.Lorem.Word())
            .RuleFor(debit => debit.Amount, f => (long)f.Finance.Amount(max: 10000))
            .RuleFor(debit => debit.DebitType, f => f.PickRandom<DebitType>())
            .RuleFor(debit => debit.PaidOn, f => f.Date.Soon())
            .RuleFor(debit => debit.UseBusinessDay, f => f.Random.Bool())
            .RuleFor(debit => debit.UserId, user.Id);
    }
}
