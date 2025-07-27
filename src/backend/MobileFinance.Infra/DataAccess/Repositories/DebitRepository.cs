using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.Debit;

namespace MobileFinance.Infra.DataAccess.Repositories;
public class DebitRepository : IDebitWriteOnlyRepository
{
    private readonly MobileFinanceDbContext _dbContext;

    public DebitRepository(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(Debit debit) => await _dbContext.Debits.AddAsync(debit);
}
