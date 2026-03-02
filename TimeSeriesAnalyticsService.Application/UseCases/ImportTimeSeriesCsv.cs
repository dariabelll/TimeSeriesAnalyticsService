using TimeSeriesAnalyticsService.Application.Services;
using TimeSeriesAnalyticsService.Domain.Models;
using TimeSeriesAnalyticsService.Domain.Services;

namespace TimeSeriesAnalyticsService.Application.UseCases;

public sealed class ImportTimeSeriesCsv(TimeSeriesCsvParser parser, IResultsCalculator calculator, ITimeSeriesStorage storage)
{
    public async Task<ImportTimeSeriesResult> ExecuteAsync(
        string fileName,
        Stream csvStream,
        CancellationToken cancellationToken)
    {
        var values = await parser.ParseCsvAsync(csvStream, cancellationToken);

        var result = calculator.Calculate(values);

        await storage.SaveAsync(fileName, values, result, cancellationToken);

        return new ImportTimeSeriesResult(values, result);
    }
}

public sealed record ImportTimeSeriesResult(IReadOnlyList<TimeSeriesValue> Values, TimeSeriesResult Result);
