using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Debit;

namespace MobileFinance.Infra.DataAccess.Repositories;
public class DebitRepository : IDebitWriteOnlyRepository, IDebitReadOnlyRepository, IDebitUpdateOnlyRepository
{
    private readonly MobileFinanceDbContext _dbContext;

    public DebitRepository(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Debit debit) => await _dbContext.Debits.AddAsync(debit);

    public async Task Delete(long debitId)
    {
        var debit = await _dbContext.Debits.FirstAsync(debit => debit.Active && debit.Id.Equals(debitId));
        _dbContext.Debits.Remove(debit);
    }

    async Task<Debit?> IDebitReadOnlyRepository.GetById(User user, long debitId) => await GetFullDebit(user, debitId, trackQuery: false);

    async Task<Debit?> IDebitUpdateOnlyRepository.GetById(User user, long debitId) => await GetFullDebit(user, debitId, trackQuery: true);

    public void Update(Debit debit) => _dbContext.Debits.Update(debit);

    private async Task<Debit?> GetFullDebit(User user, long debitId, bool trackQuery)
    {
        var query = _dbContext.Debits.AsQueryable();

        if(!trackQuery)
            query = query.AsNoTracking();

        var debit = await query.FirstOrDefaultAsync(debit => debit.Active && debit.Id.Equals(debitId) && debit.UserId.Equals(user.Id));

        return debit;
    }
}
