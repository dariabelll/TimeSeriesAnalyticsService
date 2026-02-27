namespace TimeSeriesAnalyticsService.Application.Services;

public sealed class CsvValidationException : Exception
{
    public int Row { get; }
    public string Reason { get; }

    public CsvValidationException(int row, string reason, string message)
        : base(message)
    {
        Row = row;
        Reason = reason;
    }
}