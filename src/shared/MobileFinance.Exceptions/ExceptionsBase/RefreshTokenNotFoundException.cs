using System.Net;

namespace MobileFinance.Exceptions.ExceptionsBase;
public class RefreshTokenNotFoundException : MobileFinanceException
{
    public RefreshTokenNotFoundException() : base(ExceptionMessages.INVALID_SESSION) { }

    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
