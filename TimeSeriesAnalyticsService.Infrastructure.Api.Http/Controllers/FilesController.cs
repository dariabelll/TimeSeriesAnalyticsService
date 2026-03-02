using Microsoft.AspNetCore.Mvc;
using TimeSeriesAnalyticsService.Application.Services;

namespace TimeSeriesAnalyticsService.Infrastructure.Controllers;

[ApiController]
[Route("api/files")]
public sealed class FilesController(ITimeSeriesStorage storage) : ControllerBase
{
    [HttpGet("{fileName}/values/latest")]
    public async Task<IActionResult> Latest(
        [FromRoute] string fileName,
        int count = 10,
        CancellationToken cancellationToken = default)
    {
        var values = await storage.GetLastAsync(fileName, count, cancellationToken);
        return Ok(values);
    }
}