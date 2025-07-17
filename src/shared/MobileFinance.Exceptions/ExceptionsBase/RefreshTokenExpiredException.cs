using System.Net;

namespace MobileFinance.Exceptions.ExceptionsBase;
public class RefreshTokenExpiredException : MobileFinanceException
{
    public RefreshTokenExpiredException() : base(ExceptionMessages.EXPIRED_SESSION) { }

    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
