using CommonTestUtilities.Tokens;
using Shouldly;
using System.Text.Json;

namespace WebApi.Test.Calendar.GetDay;
public class GetCalendarDayTest : MobileFinanceClassFixture
{
    private const string ROUTE = "calendar/day";
    private readonly Guid _userIdentifier;
    private readonly DateTime _incomeReceivedOn;

    public GetCalendarDayTest(MobileFinanceWebApplicationFactory factory) : base(factory)
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
        var day = _incomeReceivedOn.Day;

        var response = await DoGet(route: $"{ROUTE}?date={year}-{month}-{day}", token: token);

        response.StatusCode.ShouldBe(System.Net.HttpStatusCode.OK);

        await using var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("date").GetDateTime().ShouldBe(_incomeReceivedOn.Date);
        responseData.RootElement.GetProperty("incomes").EnumerateArray().ShouldNotBeEmpty();
    }
}
