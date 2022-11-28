using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CSharpFunctionalExtensions;
using HeldInvoiceReleaser.Api.Shared.Requests;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Models.Queries;
using HeldInvoiceReleaser.Models;

namespace HeldInvoiceReleaser.Maui.Services;

public interface IInvoiceApiService
{
    Task<Result<string>> Login(LoginCommand loginCommand);
    Task<Result<IEnumerable<HeldInvoice>>> GetHeldInvoicesByLocation(GetHeldInvoicesByLocationQuery query);
    Task<Result> ReleaseHeldInvoice(ReleaseHeldInvoiceCommand command);
}

public class InvoiceApiService : IInvoiceApiService
{
    public async Task<Result<string>> Login(LoginCommand loginCommand)
    {
        var client = await CreateClient();
        try
        {
            var response = await client.PostAsync($"{loginCommand.ServerAddress}/auth", JsonContent.Create(new AuthRequest{Location = loginCommand.LocationId}));
            return response.IsSuccessStatusCode
                ? Result.Success(await response.Content.ReadAsStringAsync())
                : Result.Failure<string>(response.StatusCode.ToString());
        }
        catch (Exception ex)
        {
            return Result.Failure<string>(ex.Message);
        }
    }

    private async Task<HttpClient> CreateClient()
    {
        var client = new HttpClient(GetInsecureHandler())
        {
            Timeout = TimeSpan.FromSeconds(7)
        };
        client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync("token"));
        return client;
    }

    public async Task<Result<IEnumerable<HeldInvoice>>> GetHeldInvoicesByLocation(GetHeldInvoicesByLocationQuery query)
    {
        var client = await CreateClient();
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

    public async Task<Result> ReleaseHeldInvoice(ReleaseHeldInvoiceCommand command)
    {

        var client = await CreateClient();
        try
        {
            var response = await client.PostAsync($"{command.ServerAddress}/location/{command.LocationId}/invoices/release/pickTicket", 
                JsonContent.Create(new ReleasePicketTicketInvoiceRequest{PicketTicket = command.PickTicket}));
            return response.IsSuccessStatusCode
                ? Result.Success()
                : Result.Failure($"Failed to get data from server. {await ExtractErrorFromResponse(response)}");
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

    private HttpMessageHandler GetInsecureHandler()
    {
#if ANDROID
        CustomAndroidMessageHandler handler = new();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        return handler;
#else
        return null;
#endif
    }
#if ANDROID
    internal sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
    {
        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
            => new CustomHostnameVerifier();

        private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
        {
            public bool Verify(string? hostname, Javax.Net.Ssl.ISSLSession? session)
            {
                return true;
            }
        }
    }
#endif
}
