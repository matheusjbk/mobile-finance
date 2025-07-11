using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.User;

namespace MobileFinance.Infra.DataAccess.Repositories;
public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly MobileFinanceDbContext _dbContext;

    public UserRepository(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> ExistActiveUserWithEmail(string email) =>
        await _dbContext.Users.AnyAsync(user => user.Active && user.Email == email);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) =>
        await _dbContext.Users.AnyAsync(user => user.Active && user.UserIdentifier.Equals(userIdentifier));

    public async Task<User?> GetByEmail(string email) =>
        await _dbContext.Users.FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);
}
