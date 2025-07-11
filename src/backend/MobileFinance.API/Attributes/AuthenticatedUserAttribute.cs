using Microsoft.AspNetCore.Mvc;
using MobileFinance.API.Filters;

namespace MobileFinance.API.Attributes;
/// <summary>
/// Authorization filter that validates the access token from the request header and ensures the user exists and is active.
/// </summary>
/// <remarks>
/// Returns a 401 Unauthorized response if the token is missing, expired or invalid.
/// </remarks>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthenticatedUserAttribute : TypeFilterAttribute
{
    public AuthenticatedUserAttribute() : base(typeof(AuthenticatedUserFilter)) { }
}
