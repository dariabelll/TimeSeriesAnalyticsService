using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TimeSeriesAnalyticsService.Application.UseCases;
using TimeSeriesAnalyticsService.Application.Services.Executions;

namespace TimeSeriesAnalyticsService.Infrastructure.Controllers;

[ApiController]
[Route("api/import")]
public class CsvController(ImportTimeSeriesCsv importerCsv) : ControllerBase
{
    [HttpPost("{fileName}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportTimeSeries(
        [FromRoute] string fileName,
        [FromForm] IFormFile csvFile, 
        CancellationToken cancellationToken)
    {
        if (csvFile == null) return BadRequest("File is required");

        if (csvFile.Length == 0) return BadRequest("File is empty");

        try
        {
            using var stream = csvFile.OpenReadStream();

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