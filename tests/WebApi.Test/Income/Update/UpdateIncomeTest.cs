using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Income.Update;
public class UpdateIncomeTest : MobileFinanceClassFixture
{
    private const string ROUTE = "income";
    private readonly Guid _userIdentifier;
    private readonly string _incomeId;

    public UpdateIncomeTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _incomeId = factory.GetIncomeId();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestIncomeJsonBuilder.Build();

        var response = await DoPut(route: $"{ROUTE}/{_incomeId}", request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Income_NotFound(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestIncomeJsonBuilder.Build();
        var id = IdEncoderBuilder.Build().Encode(1000);

        var response = await DoPut(route: $"{ROUTE}/{id}", request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("INCOME_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestIncomeJsonBuilder.Build();
        request.Title = string.Empty;

        var response = await DoPut(route: $"{ROUTE}/{_incomeId}", request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("EMPTY_INCOME_TITLE", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
