using CommonTestUtilities.Requests;
using CommonTestUtilities.Tokens;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Income.Register;
public class RegisterIncomeTest : MobileFinanceClassFixture
{
    private const string ROUTE = "income";
    private readonly Guid _userIdentifier;

    public RegisterIncomeTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestIncomeJsonBuilder.Build();

        var response = await DoPost(route: ROUTE, request: request, token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        responseData.RootElement.GetProperty("title").GetString().ShouldBe(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Empty_Title(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var request = RequestIncomeJsonBuilder.Build();
        request.Title = string.Empty;

        var response = await DoPost(route: ROUTE, request: request, token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("EMPTY_INCOME_TITLE", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
