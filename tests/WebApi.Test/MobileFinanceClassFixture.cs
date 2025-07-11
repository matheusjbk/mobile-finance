using System.Net.Http.Json;

namespace WebApi.Test;
public class MobileFinanceClassFixture : IClassFixture<MobileFinanceWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MobileFinanceClassFixture(MobileFinanceWebApplicationFactory factory) => _client = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string route, object request, string culture = "en")
    {
        ChangeRequestCulture(culture);

        return await _client.PostAsJsonAsync(route, request);
    }

    private void ChangeRequestCulture(string culture)
    {
        if(_client.DefaultRequestHeaders.Contains("Accept-Language"))
            _client.DefaultRequestHeaders.Remove("Accept-Language");

        _client.DefaultRequestHeaders.Add("Accept-Language", culture);
    }
}
