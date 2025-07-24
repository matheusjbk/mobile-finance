using System.Net;

namespace MobileFinance.Exceptions.ExceptionsBase;
public class NotFoundException : MobileFinanceException
{
    public NotFoundException(string message) : base(message) { }

    public override IList<string> GetErrorMessages() => [Message];
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
