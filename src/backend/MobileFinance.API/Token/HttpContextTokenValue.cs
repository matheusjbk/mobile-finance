using MobileFinance.Domain.Security.Tokens;

namespace MobileFinance.API.Token;
// Gets the JWT from the Authorization header of the current HTTP request.
public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HttpContextTokenValue(IHttpContextAccessor contextAccessor) => _contextAccessor = contextAccessor;

    public string Value()
    {
        var authorization = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        return authorization["Bearer ".Length..].Trim();
    }
}
