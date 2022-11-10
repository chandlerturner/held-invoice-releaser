using HeldInvoiceReleaser.Api.Services;
using HeldInvoiceReleaser.Api.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HeldInvoiceReleaser.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly ILogger<AuthController> _logger;
    private readonly ITokenService _tokenService;

    public AuthController(ILogger<AuthController> logger, ITokenService tokenService)
    {
        _logger = logger;
        _tokenService = tokenService;
    }

    [HttpPost]
    public async Task<IActionResult> Post(AuthRequest request, CancellationToken cancellationToken = default)
    {
        var token = await Task.Run(() => _tokenService.BuildToken(request.Location), cancellationToken);
        return Ok(token);
    }
}
