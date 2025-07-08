using MobileFinance.Domain.Repositories;

namespace MobileFinance.Infra.DataAccess;
public class UnitOfWork : IUnitOfWork
{
    private readonly MobileFinanceDbContext _dbContext;

    public UnitOfWork(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task Commit() => await _dbContext.SaveChangesAsync();
}
