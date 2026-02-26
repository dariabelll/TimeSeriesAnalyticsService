using TimeSeriesAnalyticsService.Domain.Models;

namespace TimeSeriesAnalyticsService.Domain.Services;

public interface IResultsCalculator
{
    public TimeSeriesResult Calculate(IReadOnlyList<TimeSeriesValue> timeSeriesValue);

}