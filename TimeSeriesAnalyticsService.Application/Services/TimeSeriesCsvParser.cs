using FluentValidation;
using TimeSeriesAnalyticsService.Domain.Models;
using System.Globalization;
using TimeSeriesAnalyticsService.Application.Services.Executions;

namespace TimeSeriesAnalyticsService.Application.Services;

public class TimeSeriesCsvParser(IValidator<TimeSeriesValue> validator)
{
    public async Task<IReadOnlyList<TimeSeriesValue>> ParseCsvAsync(Stream stream, CancellationToken cancellationToken)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));

        if (!stream.CanRead) throw new ArgumentException("Stream is not readable", nameof(stream));

        var values = new List<TimeSeriesValue>();

        using var reader = new StreamReader(stream, leaveOpen: true);

        string? line;
        int rowNumber = 0;

        while ((line = await reader.ReadLineAsync(cancellationToken)) != null)
        {
            ++rowNumber;

            if (string.IsNullOrWhiteSpace(line)) throw new CsvValidationException(rowNumber, "Full row", "Empty line");

            if (rowNumber == 1 && line.Trim().Equals("Date;ExecutionTime;Value")) continue;

            var columns = line.Split(';');

            if (columns.Length != 3) throw new CsvValidationException(rowNumber, "Full row", "Invalid number of columns");

            var dateValue = columns[0].Trim();
            var executionValue = columns[1].Trim();
            var value = columns[2].Trim();

            if (!DateTimeOffset.TryParseExact(
                dateValue,
                new[]
                {
                    "yyyy-MM-dd'T'HH:mm:ss.ffff'Z'",
                    "yyyy-MM-dd'T'HH-mm-ss.ffff'Z'"
                },
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out DateTimeOffset date
            ))
                throw new CsvValidationException(rowNumber, "Date", "Invalid date format");

            if (!double.TryParse(executionValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var execSeconds))
                throw new CsvValidationException(rowNumber, "ExecutionTime", "ExecutionTime must be a number");

            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var metricValue))
                throw new CsvValidationException(rowNumber, "Value", "Value must be a number");

            var timeSeriesValue = new TimeSeriesValue
            {
                Date = date,
                ExecutionTimeSeconds = execSeconds,
                Value = metricValue
            };

            var result = await validator.ValidateAsync(timeSeriesValue, cancellationToken);
            if (!result.IsValid)
            {
                var failure = result.Errors[0];

                var column = failure.PropertyName == nameof(TimeSeriesValue.ExecutionTimeSeconds)
                    ? "ExecutionTime"
                    : failure.PropertyName;

                throw new CsvValidationException(rowNumber, column, failure.ErrorMessage);
            }

            values.Add(timeSeriesValue);

            if (values.Count > 10_000) throw new CsvValidationException(rowNumber, "Row", "Row count cannot exceed 10 000");
        }

        if (values.Count == 0) throw new CsvValidationException(0, "Full file", "File is empty");

        return values;
    }

}