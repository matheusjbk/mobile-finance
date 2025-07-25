namespace MobileFinance.Application.UseCases.Income.Delete;
public interface IDeleteIncomeUseCase
{
    public Task Execute(long incomeId);
}
