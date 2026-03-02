using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeSeriesAnalyticsService.Application.UseCases;
using TimeSeriesAnalyticsService.Application.Services.Executions;

namespace TimeSeriesAnalyticsService.Infrastructure.Api.Http.Controllers;

[ApiController]
[Route("api/import")]
public class CsvController(ImportTimeSeriesCsv importerCsv) : ControllerBase
{
    [HttpPost("{fileName}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportTimeSeries(
        [FromRoute] string fileName,
        [FromForm] IFormFile file, 
        CancellationToken cancellationToken)
    {
        if (file == null) return BadRequest("File is required");

        if (file.Length == 0) return BadRequest("File is empty");

        try
        {
            using var stream = file.OpenReadStream();

            var result = await importerCsv.ExecuteAsync(fileName, stream, cancellationToken);

            return Ok(result);
        }
        catch (CsvValidationException ex)
        {
            return BadRequest(new
            {
                row = ex.Row,
                reason = ex.Reason,
                message = ex.Message
            });
        }
        
    }
}