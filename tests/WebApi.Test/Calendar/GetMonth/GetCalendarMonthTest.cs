using CommonTestUtilities.Tokens;
using Shouldly;
using System.Text.Json;

namespace WebApi.Test.Calendar.GetMonth;
public class GetCalendarMonthTest : MobileFinanceClassFixture
{
    private const string ROUTE = "calendar/month";
    private readonly Guid _userIdentifier;
    private readonly DateTime _incomeReceivedOn;

    public GetCalendarMonthTest(MobileFinanceWebApplicationFactory factory) : base(factory)
    {
        _userIdentifier = factory.GetUserIdentifier();
        _incomeReceivedOn = factory.GetIncomeReveicedDate();
    }

    [Fact]
    public async Task Success()
    {
        var token = AccessTokenGeneratorBuilder.Build().Generate(_userIdentifier);
        var year = _incomeReceivedOn.Year;
        var month = _incomeReceivedOn.Month;

        var response = await DoGet(route: $"{ROUTE}?year={year}&month={month}", token: token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("year").GetInt32().ShouldBe(year);
        responseData.RootElement.GetProperty("month").GetInt32().ShouldBe(month);
        responseData.RootElement.GetProperty("days").EnumerateArray().ShouldNotBeEmpty();
    }
}
