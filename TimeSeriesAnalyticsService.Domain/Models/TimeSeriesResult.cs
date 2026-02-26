namespace TimeSeriesAnalyticsService.Domain.Models;

public sealed class TimeSeriesResult
{
    public double DeltaSeconds { get; init; }
    public DateTimeOffset FirstStart { get; init; }
    public double AverageExecutionTimeSeconds { get; init; }
    public double AverageValue { get; init; }
    public double MedianValue { get; init; }
    public double MaxValue { get; init; }
    public double MinValue { get; init; }
    public int RowCount { get; init; }
}