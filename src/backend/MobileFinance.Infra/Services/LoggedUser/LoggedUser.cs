using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Security.Tokens;
using MobileFinance.Domain.Services.LoggedUser;
using MobileFinance.Infra.DataAccess;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MobileFinance.Infra.Services.LoggedUser;
// Service that retrieves the currently authenticated and active user from the database using the JWT token.
public class LoggedUser : ILoggedUser
{
    private readonly ITokenProvider _tokenProvider;
    private readonly MobileFinanceDbContext _dbContext;

    public LoggedUser(ITokenProvider tokenProvider, MobileFinanceDbContext dbContext)
    {
        _tokenProvider = tokenProvider;
        _dbContext = dbContext;
    }

    public async Task<User> GetUser()
    {
        var token = _tokenProvider.Value();

        var tokenHandler = new JwtSecurityTokenHandler();

        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

        var identifier = jwtSecurityToken.Claims.First(c => c.Type == ClaimTypes.Sid).Value;

        var userIdentifier = Guid.Parse(identifier);

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.Active && user.UserIdentifier.Equals(userIdentifier));
    }
}
