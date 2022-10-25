using CSharpFunctionalExtensions;
using HeldInvoiceReleaser.Maui.Models.Commands;

namespace HeldInvoiceReleaser.Maui.Services;

public interface IInvoiceApiService
{
    Task<Result> Login(LoginCommand loginCommand);
}

public class InvoiceApiService : IInvoiceApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public InvoiceApiService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }
    public async Task<Result> Login(LoginCommand loginCommand)
    {
        var client = _httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync($"{loginCommand.Server}/auth");
            return response.IsSuccessStatusCode
                ? Result.Success()
                : Result.Failure(response.StatusCode.ToString());
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }
}
