using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.User.Update;
public class UpdateUserInvalidTokenTest : MobileFinanceClassFixture
{
    private const string ROUTE = "user";

    public UpdateUserInvalidTokenTest(MobileFinanceWebApplicationFactory factory) : base(factory) { }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(route: ROUTE, request: request, token: "tokenInvalid", culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("USER_WITHOUT_PERMISSION", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Token(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(route: ROUTE, request: request, token: string.Empty, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("NO_TOKEN", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Valid_Token_User_NotFound(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var token = AccessTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());

        var response = await DoPut(route: ROUTE, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("USER_WITHOUT_PERMISSION", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
