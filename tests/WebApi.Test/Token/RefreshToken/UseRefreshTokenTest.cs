using CommonTestUtilities.Requests;
using MobileFinance.Communication.Requests;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Token.RefreshToken;
public class UseRefreshTokenTest : MobileFinanceClassFixture
{
    private const string ROUTE = "token/refresh-token";
    private readonly MobileFinance.Domain.Entities.RefreshToken _refreshToken;

    public UseRefreshTokenTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _refreshToken = factory.GetRefreshToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestNewTokenJson { RefreshToken = _refreshToken.Value };

        var response = await DoPost(route: ROUTE, request: request);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.ShouldSatisfyAllConditions(
            () => responseData.RootElement.GetProperty("accessToken").GetString().ShouldNotBeNullOrEmpty(),
            () => responseData.RootElement.GetProperty("refreshToken").GetString().ShouldNotBeNullOrEmpty());
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_RefreshToken_NotFound(string culture)
    {
        var request = RequestNewTokenJsonBuilder.Build();

        var response = await DoPost(route: ROUTE, request: request, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("INVALID_SESSION", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
