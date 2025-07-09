using System.Net;

namespace MobileFinance.Exceptions.ExceptionsBase;
public class ErrorOnValidationException : MobileFinanceException
{
    private readonly IList<string> _errorMessages;

    public ErrorOnValidationException(IList<string> errorMessages) : base(string.Empty) => _errorMessages = errorMessages;

    public override IList<string> GetErrorMessages() => _errorMessages;
    public override HttpStatusCode GetStatusCode() => HttpStatusCode.BadRequest;
}
