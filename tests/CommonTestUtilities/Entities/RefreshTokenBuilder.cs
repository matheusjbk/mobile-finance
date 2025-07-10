using MobileFinance.Domain.Entities;

namespace CommonTestUtilities.Entities;
public class RefreshTokenBuilder
{
    public static RefreshToken Build(User user)
    {
        return new RefreshToken
        {
            Id = 1,
            Value = Convert.ToBase64String(Guid.NewGuid().ToByteArray()),
            UserId = user.Id,
            User = user,
        };
    }
}
