using HeldInvoiceReleaser.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeldInvoiceReleaser.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class InvoiceController : ControllerBase
{

    private readonly ILogger<InvoiceController> _logger;

    public InvoiceController(ILogger<InvoiceController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<HeldInvoice> Get()
    {
        return Enumerable.Empty<HeldInvoice>();
    }
}
