using HeldInvoiceReleaser.Api.Services;
using HeldInvoiceReleaser.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeldInvoiceReleaser.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{

    private readonly ILogger<InvoiceController> _logger;
    private readonly IDatabaseService _databaseService;

    public InvoiceController(ILogger<InvoiceController> logger, IDatabaseService databaseService)
    {
        _logger = logger;
        _databaseService = databaseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HeldInvoice>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get()
    {
        var result = await _databaseService.GetAllHeldInvoicesByLocation("");
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }
}
