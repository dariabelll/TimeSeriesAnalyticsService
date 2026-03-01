namespace TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Entities;

public sealed class TimeSeriesValueEntity
{
    public long Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double ExecutionTimeSeconds { get; set; }
    public double Value { get; set; }
    public required string FileName { get; set; }
}
