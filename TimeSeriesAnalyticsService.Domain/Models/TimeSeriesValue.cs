namespace TimeSeriesAnalyticsService.Domain.Models;

public sealed class TimeSeriesValue
{
    public DateTimeOffset Date { get; init; }
    public double ExecutionTimeSeconds { get; init; }
    public double Value { get; init; }
}
