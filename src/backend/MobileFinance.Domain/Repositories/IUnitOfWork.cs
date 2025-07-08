namespace MobileFinance.Domain.Repositories;
public interface IUnitOfWork
{
    public Task Commit();
}
