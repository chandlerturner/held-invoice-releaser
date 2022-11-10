using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HeldInvoiceReleaser.Api.Models.Options;
using HeldInvoiceReleaser.Api.Shared;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HeldInvoiceReleaser.Api.Services;

public interface ITokenService
{
    string BuildToken(string location);
    bool IsTokenValid(string token);
}

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;
    private readonly IOptionsMonitor<JwtOptions> _optionsMonitor;
    private string Issuer => _optionsMonitor.CurrentValue.Issuer;
    private string Key => _optionsMonitor.CurrentValue.Key;

    public TokenService(ILogger<TokenService> logger, IOptionsMonitor<JwtOptions> optionsMonitor)
    {
        _logger = logger;
        _optionsMonitor = optionsMonitor;
    }

    public string BuildToken(string location)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(JwtClaims.Location, location)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(Issuer, Issuer, claims, signingCredentials: credentials);
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }

    public bool IsTokenValid(string token)
    {
        var mySecret = Encoding.UTF8.GetBytes(Key);
        var mySecurityKey = new SymmetricSecurityKey(mySecret);
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token,
                new TokenValidationParameters
                {
                    RequireExpirationTime = false,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Issuer,
                    ValidAudience = Issuer,
                    IssuerSigningKey = mySecurityKey,
                }, out _);
        }
        catch
        {
            return false;
        }

        return true;
    }
}