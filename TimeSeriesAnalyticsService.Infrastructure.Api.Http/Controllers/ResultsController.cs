using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSeriesAnalyticsService.Infrastructure.DataAccess.EntityFramework;

namespace TimeSeriesAnalyticsService.Infrastructure.Api.Http.Controllers;

[ApiController]
[Route("api/results")]
public sealed class ResultsController(TimeSeriesDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? fileName,
        [FromQuery] DateTimeOffset? firstStartFrom,
        [FromQuery] DateTimeOffset? firstStartTo,
        [FromQuery] double? avgValueFrom,
        [FromQuery] double? avgValueTo,
        [FromQuery] double? avgExecFrom,
        [FromQuery] double? avgExecTo,
        [FromQuery] int skip = 0,
        [FromQuery] int take = 50,
        CancellationToken cancellationToken = default)
    {
        if (skip < 0) return BadRequest("skip must be positive");
        if (take <= 0 || take > 500) return BadRequest("take must be between 1 and 500");

        var query = db.Results.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(fileName))
            query = query.Where(x => x.FileName == fileName);

        if (firstStartFrom.HasValue)
            query = query.Where(x => x.FirstStart >= firstStartFrom.Value);

        if (firstStartTo.HasValue)
            query = query.Where(x => x.FirstStart <= firstStartTo.Value);

        if (avgValueFrom.HasValue)
            query = query.Where(x => x.AverageValue >= avgValueFrom.Value);

        if (avgValueTo.HasValue)
            query = query.Where(x => x.AverageValue <= avgValueTo.Value);

        if (avgExecFrom.HasValue)
            query = query.Where(x => x.AverageExecutionTimeSeconds >= avgExecFrom.Value);

        if (avgExecTo.HasValue)
            query = query.Where(x => x.AverageExecutionTimeSeconds <= avgExecTo.Value);

        var items = await query
            .OrderByDescending(x => x.UpdatedAt)
            .Skip(skip)
            .Take(take)
            .Select(x => new
            {
                x.FileName,
                x.DeltaSeconds,
                x.FirstStart,
                x.AverageExecutionTimeSeconds,
                x.AverageValue,
                x.MedianValue,
                x.MaxValue,
                x.MinValue,
                x.RowCount,
                x.UpdatedAt
            })
            .ToListAsync(cancellationToken);

        return Ok(items);
    }
}