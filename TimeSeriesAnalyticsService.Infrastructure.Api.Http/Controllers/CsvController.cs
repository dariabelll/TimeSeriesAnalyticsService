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
    public async Task<IActionResult> ImportTimeSeries([FromForm] IFormFile csvFile, CancellationToken cancellationToken)
    {
        if (csvFile == null) throw new ArgumentNullException(nameof(csvFile));

        if (csvFile.Length == 0) throw new ArgumentException("File is empty", nameof(csvFile));

        try
        {
            using var stream = csvFile.OpenReadStream();

            var result = await importerCsv.ExecuteAsync(stream, cancellationToken);

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