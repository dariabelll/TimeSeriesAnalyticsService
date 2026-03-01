using Microsoft.EntityFrameworkCore;
using TimeSeriesAnalyticsService.Domain.Models;
using TimeSeriesAnalyticsService.Application.Services;
using TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework.Entities;

namespace TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework;

public sealed class TimeSeriesStorageEf(TimeSeriesDbContext context) : ITimeSeriesStorage
{
    public async Task SaveAsync(string fileName, IReadOnlyList<TimeSeriesValue> values, 
        TimeSeriesResult result, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("Filename is required", nameof(fileName));
        
        if (values == null) throw new ArgumentNullException(nameof(values), "Values are required");
        
        if (values.Count == 0) throw new ArgumentException("Values cannot be empty", nameof(values));

        if (result == null) throw new ArgumentNullException(nameof(result), "Result is required");

        await using var transaction = await context.Database.BeginTransactionAsync(cancellationToken);

        await context.Values
            .Where(x => x.FileName == fileName)
            .ExecuteDeleteAsync(cancellationToken);

        var entities = values.Select(x => new TimeSeriesValueEntity
        {
            FileName = fileName,
            Date = x.Date,
            ExecutionTimeSeconds = x.ExecutionTimeSeconds,
            Value = x.Value
        }).ToList();

        await context.Values.AddRangeAsync(entities, cancellationToken);

        var existingResult = await context.Results.FirstOrDefaultAsync(x => x.FileName == fileName, cancellationToken);

        if (existingResult == null) 
        {
            context.Results.Add(new TimeSeriesResultEntity
            {
                FileName = fileName,
                DeltaSeconds = result.DeltaSeconds,
                FirstStart = result.FirstStart,
                AverageExecutionTimeSeconds = result.AverageExecutionTimeSeconds,
                AverageValue = result.AverageValue,
                MedianValue = result.MedianValue,
                MaxValue = result.MaxValue,
                MinValue = result.MinValue,
                RowCount = result.RowCount,
                UpdatedAt = DateTimeOffset.UtcNow
            });
        }
        else
        {
            existingResult.DeltaSeconds = result.DeltaSeconds;
            existingResult.FirstStart = result.FirstStart;
            existingResult.AverageExecutionTimeSeconds = result.AverageExecutionTimeSeconds;
            existingResult.AverageValue = result.AverageValue;
            existingResult.MedianValue = result.MedianValue;
            existingResult.MaxValue = result.MaxValue;
            existingResult.MinValue = result.MinValue;
            existingResult.RowCount = result.RowCount;
            existingResult.UpdatedAt = DateTimeOffset.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        
    }

    public async Task<IReadOnlyList<TimeSeriesValue>> GetLastAsync(string fileName, int count,CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException(nameof(fileName), "Filename is required");

        if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive");

        if (count > 1000) throw new ArgumentOutOfRangeException(nameof(count), "Count is too big");

        return await context.Values
        .AsNoTracking()
        .Where(x => x.FileName == fileName)
        .OrderByDescending(x => x.Date)
        .Take(count)
        .Select(x => new TimeSeriesValue
        {   
            Date =x.Date, 
            Value = x.Value, 
            ExecutionTimeSeconds = x.ExecutionTimeSeconds
        })
        .ToListAsync(cancellationToken);
    }
}