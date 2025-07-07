using System.Net;

namespace MobileFinance.Exceptions.ExceptionsBase;
public abstract class MobileFinanceException : SystemException
{
    protected MobileFinanceException(string message) : base(message) { }

    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}
