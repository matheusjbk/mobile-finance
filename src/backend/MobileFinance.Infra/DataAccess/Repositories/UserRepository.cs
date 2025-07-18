﻿using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories.User;

namespace MobileFinance.Infra.DataAccess.Repositories;
public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository, IUserDeleteOnlyRepository
{
    private readonly MobileFinanceDbContext _dbContext;

    public UserRepository(MobileFinanceDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> ExistActiveUserWithEmail(string email) =>
        await _dbContext.Users.AnyAsync(user => user.Active && user.Email == email);

    public async Task<bool> ExistActiveUserWithIdentifier(Guid userIdentifier) =>
        await _dbContext.Users.AnyAsync(user => user.Active && user.UserIdentifier.Equals(userIdentifier));

    public async Task<User?> GetByEmail(string email) =>
        await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Active && user.Email.Equals(email));

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<User> GetById(long userId) => await _dbContext.Users.FirstAsync(user => user.Active && user.Id.Equals(userId));

    public void Update(User user) => _dbContext.Users.Update(user);

    public async Task DeleteAccount(Guid userIdentifier)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserIdentifier.Equals(userIdentifier));

        if(user is null)
            return;

        var refreshTokens = _dbContext.RefreshTokens.Where(token => token.UserId.Equals(user.Id));

        _dbContext.RefreshTokens.RemoveRange(refreshTokens);
        _dbContext.Users.Remove(user);
    }
}
