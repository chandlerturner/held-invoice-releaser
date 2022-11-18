using HeldInvoiceReleaser.Api.Models.Commands;
using HeldInvoiceReleaser.Api.Services;
using HeldInvoiceReleaser.Api.Shared.Requests;
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

    [HttpGet("/location/{location}/invoices")]
    [ProducesResponseType(typeof(IEnumerable<HeldInvoice>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(string location)
    {
        var result = await _databaseService.GetAllHeldInvoicesByLocation(location);
        return result.IsSuccess
            ? Ok(result.Value)
            : BadRequest(result.Error);
    }

    [HttpPost("/location/{location}/invoices/release/pickTicket")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ReleaseHeldInvoice(string location, ReleasePicketTicketInvoiceRequest request)
    {
        var command = new ReleaseHeldInvoiceCommand { LocationId = location, PickTicket = request.PicketTicket };
        var result = await _databaseService.ReleaseHeldInvoicesByLocationAndPicketTicket(command);
        return result.IsSuccess
            ? Ok()
            : BadRequest(result.Error);
    }
}
