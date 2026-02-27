using TimeSeriesAnalyticsService.Application.Services;
using TimeSeriesAnalyticsService.Domain.Models;
using TimeSeriesAnalyticsService.Domain.Services;

namespace TimeSeriesAnalyticsService.Application.UseCases;

public sealed class ImportTimeSeriesCsv(TimeSeriesCsvParser parser, IResultsCalculator calculator)
{
    public async Task<ImportTimeSeriesResult> ExecuteAsync(
        Stream csvStream,
        CancellationToken cancellationToken)
    {
        var values = await parser.ParseCsvAsync(csvStream, cancellationToken);

        var result = calculator.Calculate(values);

        return new ImportTimeSeriesResult(values, result);
    }
}

public sealed record ImportTimeSeriesResult(IReadOnlyList<TimeSeriesValue> Values, TimeSeriesResult Result);
