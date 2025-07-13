using MobileFinance.Domain.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace WebApi.Test;
public class MobileFinanceClassFixture : IClassFixture<MobileFinanceWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MobileFinanceClassFixture(MobileFinanceWebApplicationFactory factory) => _client = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string route, object request, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizationRequest(token);

        return await _client.PostAsJsonAsync(route, request);
    }

    protected async Task<HttpResponseMessage> DoGet(string route, string token = "", string culture = "en")
    {
        ChangeRequestCulture(culture);
        AuthorizationRequest(token);

        return await _client.GetAsync(route);
    }

    private void ChangeRequestCulture(string culture)
    {
        if(_client.DefaultRequestHeaders.Contains("Accept-Language"))
            _client.DefaultRequestHeaders.Remove("Accept-Language");

        _client.DefaultRequestHeaders.Add("Accept-Language", culture);
    }

    private void AuthorizationRequest(string token)
    {
        if(!token.NotEmpty())
            return;

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
