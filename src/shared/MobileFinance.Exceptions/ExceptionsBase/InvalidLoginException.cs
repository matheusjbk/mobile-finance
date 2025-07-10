using System.Net;

namespace MobileFinance.Exceptions.ExceptionsBase;
public class InvalidLoginException : MobileFinanceException
{
    public InvalidLoginException() : base(ExceptionMessages.INVALID_EMAIL_OR_PASSWORD) { }

    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Unauthorized;
}
