using CommonTestUtilities.Tokens;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.User.Profile;
public class GetUserProfileTest : MobileFinanceClassFixture
{
    private const string ROUTE = "user";
    private readonly string _userName;
    private readonly string _userEmail;
    private readonly Guid _userIdentifier;

    public GetUserProfileTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userName = factory.GetUserName();
        _userEmail = factory.GetUserEmail();
        _userIdentifier = factory.GetUserIdentifier();
    }

    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(route: ROUTE, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.ShouldSatisfyAllConditions(
            () => responseData.RootElement.GetProperty("name").GetString().ShouldNotBeNullOrWhiteSpace(),
            () => responseData.RootElement.GetProperty("name").GetString().ShouldBe(_userName),
            () => responseData.RootElement.GetProperty("email").GetString().ShouldNotBeNullOrWhiteSpace(),
            () => responseData.RootElement.GetProperty("email").GetString().ShouldBe(_userEmail));
    }
}
