namespace TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Entities;

public sealed class TimeSeriesResultEntity
{
    public required string FileName { get; set; }
    public double DeltaSeconds { get; set; }
    public DateTimeOffset FirstStart { get; set; }
    public double AverageExecutionTimeSeconds { get; set; }
    public double AverageValue { get; set; }
    public double MedianValue { get; set; }
    public double MaxValue { get; set; }
    public double MinValue { get; set; }
    public int RowCount { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}