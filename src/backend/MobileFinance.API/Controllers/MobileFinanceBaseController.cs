using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace MobileFinance.API.Controllers;
[Route("[controller]")]
[ApiController]
public class MobileFinanceBaseController : ControllerBase
{
    protected static bool IsUserAuthenticated(AuthenticateResult authenticate) => authenticate.Succeeded
            || authenticate.Principal is not null
            || authenticate.Principal!.Identities.Any(id => id.IsAuthenticated);
}
