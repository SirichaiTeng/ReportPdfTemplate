using Microsoft.AspNetCore.Mvc;
using ReportPdfTemplate.Models;
using ReportPdfTemplate.Models.Response;
using ReportPdfTemplate.Services;

namespace ReportPdfTemplate.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ReportController : ControllerBase
{
    // Install NuGet iText7
    [HttpPost("documentsale")]
    public async Task<IActionResult> CreateDocumentSale([FromBody] RequestDto request, [FromServices] IGenerateDocument generateDocument)
    {
        var result = generateDocument.CreateSaleRequest(request);
        var FileName = DateTime.Now.AddHours(7).ToString("dd-MM-yyyy_HHmmss");
        return File(result.ToArray(), "appication/pdf", $"Sale_{FileName}");
    }
}
