using System.Net;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Models.Queries;
using HeldInvoiceReleaser.Models;

namespace HeldInvoiceReleaser.Maui.Services;

public interface IInvoiceApiService
{
    Task<Result> Login(LoginCommand loginCommand);
    Task<Result<IEnumerable<HeldInvoice>>> GetHeldInvoicesByLocation(GetHeldInvoicesByLocationQuery query);
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
            var response = await client.PostAsync($"{loginCommand.ServerAddress}/auth", JsonContent.Create(new {location = loginCommand.Location}));
            return response.IsSuccessStatusCode
                ? Result.Success()
                : Result.Failure(response.StatusCode.ToString());
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message);
        }
    }

    public async Task<Result<IEnumerable<HeldInvoice>>> GetHeldInvoicesByLocation(GetHeldInvoicesByLocationQuery query)
    {
        var client = _httpClientFactory.CreateClient();
        try
        {
            var response = await client.GetAsync($"{query.ServerAddress}/location/{query.Location}/invoices");
            return response.IsSuccessStatusCode
                ? await ExtractInvoicesFromResponse(response)
                : Result.Failure<IEnumerable<HeldInvoice>>($"Failed to get data from server. {await ExtractErrorFromResponse(response)}");
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<HeldInvoice>>(ex.Message);
        }
    }

    private async Task<Result<IEnumerable<HeldInvoice>>> ExtractInvoicesFromResponse(HttpResponseMessage response)
    {
        try
        {
            var invoices = await response.Content.ReadFromJsonAsync<IEnumerable<HeldInvoice>>();
            return Result.Success(invoices);
        }
        catch (Exception ex)
        {
            return Result.Failure<IEnumerable<HeldInvoice>>($"Unable to extract data from response. {ex.Message}");
        }
    }

    private async Task<string> ExtractErrorFromResponse(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return await response.Content.ReadAsStringAsync();
        }

        return response.StatusCode.ToString();
    }
}
