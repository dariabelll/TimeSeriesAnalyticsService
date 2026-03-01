using TimeSeriesAnalyticsService.Domain.Models;

namespace TimeSeriesAnalyticsService.Application.Services;

public interface ITimeSeriesStorage
{
    Task SaveAsync(string fileName, IReadOnlyList<TimeSeriesValue> values, 
        TimeSeriesResult result, CancellationToken cancellationToken);

    Task<IReadOnlyList<TimeSeriesValue>> GetLastAsync(string fileName, int count, CancellationToken cancellationToken);
}