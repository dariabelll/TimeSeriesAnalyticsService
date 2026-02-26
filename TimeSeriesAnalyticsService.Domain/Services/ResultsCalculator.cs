using TimeSeriesAnalyticsService.Domain.Models;

namespace TimeSeriesAnalyticsService.Domain.Services;

public class ResultsCalculator : IResultsCalculator
{
    public TimeSeriesResult Calculate(IReadOnlyList<TimeSeriesValue> timeSeriesValue)
    {
        if (timeSeriesValue == null)
        {
            throw new ArgumentNullException(nameof(timeSeriesValue));
        }

        if (timeSeriesValue.Count == 0)
        {
            throw new ArgumentException("List cannot be empty", nameof(timeSeriesValue));
        }

        var maxDate = timeSeriesValue.Max(x => x.Date);
        var minDate = timeSeriesValue.Min(x => x.Date);
        var deltaDate = (maxDate - minDate).TotalSeconds;

        var avgExecutionTime = timeSeriesValue.Average(x => x.ExecutionTimeSeconds);

        var avgValue = timeSeriesValue.Average(x => x.Value);

        var maxValue = timeSeriesValue.Max(x => x.Value);
        var minValue = timeSeriesValue.Min(x => x.Value);

        var size = timeSeriesValue.Count;

        var medianValue = CalculateMedian(timeSeriesValue.Select(x => x.Value).ToArray(), size);

        return new TimeSeriesResult
        {
            DeltaSeconds = deltaDate,
            FirstStart = minDate,
            AverageExecutionTimeSeconds = avgExecutionTime,
            AverageValue = avgValue,
            MedianValue = medianValue,
            MaxValue = maxValue,
            MinValue = minValue,
            RowCount = size
        };
    }


    private static double CalculateMedian(double[] values, int size)
    {
        Array.Sort(values);

        return size % 2 == 0 ? (values[size / 2] + values[size / 2 - 1]) / 2 : values[size / 2];
    }
}
