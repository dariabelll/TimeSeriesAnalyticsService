using FluentValidation;
using TimeSeriesAnalyticsService.Domain.Models;

namespace TimeSeriesAnalyticsService.Application.Validation;

public sealed class TimeSeriesValidator: AbstractValidator<TimeSeriesValue>
{
    private static readonly DateTimeOffset MinAllowedDateUtc = new(new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc));

    public TimeSeriesValidator()
    {
        RuleFor(x => x.Date).NotNull().WithMessage("Date is required");

        RuleFor(x => x.ExecutionTimeSeconds).NotNull().WithMessage("Execution time is required");

        RuleFor(x => x.Value).NotNull().WithMessage("Value is required");

        RuleFor(x => x.Date).GreaterThanOrEqualTo(MinAllowedDateUtc).WithMessage("Date must be greater than 2000-01-01");

        RuleFor(x => x.Date).LessThanOrEqualTo(DateTimeOffset.UtcNow).WithMessage("Date must be less than current date");

        RuleFor(x => x.ExecutionTimeSeconds).GreaterThanOrEqualTo(0).WithMessage("Execution time must be greater than 0");

        RuleFor(x => x.Value).GreaterThanOrEqualTo(0).WithMessage("Value must be greater than 0");
    }

}