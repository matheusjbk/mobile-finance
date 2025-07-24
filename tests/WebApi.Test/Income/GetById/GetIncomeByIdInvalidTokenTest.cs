using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Income.GetById;
public class GetIncomeByIdInvalidTokenTest : MobileFinanceClassFixture
{
    private const string ROUTE = "income";

    public GetIncomeByIdInvalidTokenTest(MobileFinanceWebApplicationFactory factory) : base(factory) { }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Token(string culture)
    {
        var id = IdEncoderBuilder.Build().Encode(1);
        var response = await DoGet(route: $"{ROUTE}/{id}", token: "tokenInvalid", culture: culture);

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
        var id = IdEncoderBuilder.Build().Encode(1);
        var response = await DoGet(route: $"{ROUTE}/{id}", token: string.Empty, culture: culture);

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
        var token = AccessTokenGeneratorBuilder.Build().Generate(Guid.NewGuid());
        var id = IdEncoderBuilder.Build().Encode(1);
        var response = await DoGet(route: $"{ROUTE}/{id}", token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("USER_WITHOUT_PERMISSION", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
