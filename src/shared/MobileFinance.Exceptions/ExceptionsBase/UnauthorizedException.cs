using System.Net;

namespace MobileFinance.Exceptions.ExceptionsBase;
public class UnauthorizedException : MobileFinanceException
{
    public UnauthorizedException(string message) : base(message) { }

    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
