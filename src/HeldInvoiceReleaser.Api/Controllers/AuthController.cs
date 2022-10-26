using HeldInvoiceReleaser.Api.Services;
using HeldInvoiceReleaser.Models;
using Microsoft.AspNetCore.Mvc;

namespace HeldInvoiceReleaser.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Post()
    {
        return Ok();
    }
}
