using MobileFinance.Domain.Extensions;
using System.Globalization;

namespace MobileFinance.API.Middlewares;
// Middleware that sets the request culture based on the Accept-Language header.
public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures);
        var requestedLanguage = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        var cultureInfo = new CultureInfo("en");

        if(requestedLanguage.NotEmpty() && supportedLanguages.Any(c => c.Name.Equals(requestedLanguage)))
            cultureInfo = new CultureInfo(requestedLanguage);

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
