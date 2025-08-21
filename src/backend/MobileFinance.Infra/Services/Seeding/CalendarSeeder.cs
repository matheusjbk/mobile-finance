using Microsoft.EntityFrameworkCore;
using MobileFinance.Domain.Entities;
using MobileFinance.Domain.Repositories;
using MobileFinance.Infra.DataAccess;
using System.Globalization;
using System.Text.Json;

namespace MobileFinance.Infra.Services.Seeding;
public class CalendarSeeder
{
    private readonly HttpClient _client;
    private readonly MobileFinanceDbContext _dbContext;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public CalendarSeeder(
        HttpClient client,
        MobileFinanceDbContext dbContext,
        IUnitOfWork unitOfWork)
    {
        _client = client;
        _dbContext = dbContext;
        _unitOfWork = unitOfWork;
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task Seed()
    {
        var currentYear = DateTime.UtcNow.Year;
        var nextYear = currentYear + 1;

        await AddYearIfMissing(currentYear);
        await AddYearIfMissing(nextYear);
    }

    private async Task AddYearIfMissing(int year)
    {
        var yearExists = await _dbContext.CalendarDays
            .AnyAsync(day => day.Date.Year.Equals(year));

        if(!yearExists)
        {
            var response = await _client.GetAsync($"https://brasilapi.com.br/api/feriados/v1/{year}");
            var holidaysJson = await response.Content.ReadAsStringAsync();
            var holidays = JsonSerializer.Deserialize<List<BrasilApiHoliday>>(holidaysJson, _jsonSerializerOptions) ?? [];
            var holidayDates = holidays.Select(holiday => DateTime.Parse(holiday.Date, new CultureInfo("pt-BR"))).ToHashSet();

            var days = Enumerable
                .Range(1, DateTime.IsLeapYear(year) ? 366 : 365)
                .Select(day =>
                {
                    var date = new DateTime(year, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc).AddDays(day - 1);
                    var isHoliday = holidayDates.Contains(date);
                    var isWeekend = date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

                    return new CalendarDay
                    {
                        Date = date,
                        IsHoliday = isHoliday,
                        IsWeekend = isWeekend,
                        IsBusinessDay = !isHoliday && !isWeekend
                    };
                });

            _dbContext.CalendarDays.AddRange(days);
            await _unitOfWork.Commit();
        }
    }
}
