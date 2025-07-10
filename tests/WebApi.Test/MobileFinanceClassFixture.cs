using System.Net.Http.Json;

namespace WebApi.Test;
public class MobileFinanceClassFixture : IClassFixture<MobileFinanceWebApplicationFactory>
{
    private readonly HttpClient _client;

    public MobileFinanceClassFixture(MobileFinanceWebApplicationFactory factory) => _client = factory.CreateClient();

    protected async Task<HttpResponseMessage> DoPost(string route, object request) => await _client.PostAsJsonAsync(route, request);
}
