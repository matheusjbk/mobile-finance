using CommonTestUtilities.Cryptography;
using CommonTestUtilities.Tokens;
using MobileFinance.Exceptions;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Income.GetById;
public class GetIncomeByIdTest : MobileFinanceClassFixture
{
    private const string ROUTE = "income";
    private readonly Guid _userIdentifier;
    private readonly string _incomeId;
    private readonly string _incomeTitle;

    public GetIncomeByIdTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _incomeId = factory.GetIncomeId();
        _incomeTitle = factory.GetIncomeTitle();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);

        var response = await DoGet(route: $"{ROUTE}/{_incomeId}", token: token);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("id").GetString().ShouldBe(_incomeId);
        responseData.RootElement.GetProperty("title").GetString().ShouldBe(_incomeTitle);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Income_NotFound(string culture)
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var id = IdEncoderBuilder.Build().Encode(1000);

        var response = await DoGet(route: $"{ROUTE}/{id}", token: token, culture: culture);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();

        var expectedMessage = ExceptionMessages.ResourceManager.GetString("INCOME_NOT_FOUND", new CultureInfo(culture));

        errors.ShouldHaveSingleItem().GetString().ShouldBe(expectedMessage);
    }
}
