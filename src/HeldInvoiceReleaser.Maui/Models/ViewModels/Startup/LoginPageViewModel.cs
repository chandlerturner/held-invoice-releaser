using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CSharpFunctionalExtensions;
using FluentValidation;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Pages;
using HeldInvoiceReleaser.Maui.Services;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty] private string _serverAddress;
    [ObservableProperty] private string _locationId;
    [ObservableProperty] private string _loginError;

    private readonly IInvoiceApiService _invoiceApiService;
    private readonly IValidator<LoginCommand> _loginCommandValidator;
    private readonly IRoutingService _routingService;

    public LoginPageViewModel(IInvoiceApiService invoiceApiService, IValidator<LoginCommand> loginCommandValidator, IRoutingService routingService)
    {
        _invoiceApiService = invoiceApiService;
        _loginCommandValidator = loginCommandValidator;
        _routingService = routingService;

        ServerAddress = Preferences.Get(nameof(ServerAddress), string.Empty);
        LocationId = Preferences.Get(nameof(Location), string.Empty);
    }

    [RelayCommand]
    public async Task Login()
    {
        LoginError = string.Empty;
        var loginResult =  await CreateCommand()
            .Bind(CallLoginApi)
            .TapError(error => LoginError = error)
            .Tap(SavePreferences)
            .Tap(SaveToken)
            .Tap(LoadPages);
    }

    private async Task SaveToken(string token)
    {
        SecureStorage.Remove("token");
        await SecureStorage.SetAsync("token", token);
    }

    private async Task LoadPages()
    {
        await _routingService.NavigateToAsync($"//{nameof(MainPage)}");
    }

    private Result<LoginCommand> CreateCommand()
    {
        LoginCommand loginCommand = new() { ServerAddress = ServerAddress, Location = LocationId };
        var validationResult = _loginCommandValidator.Validate(loginCommand);
        return validationResult.IsValid
            ? loginCommand
            : Result.Failure<LoginCommand>(
                string.Join('|', validationResult.Errors.Select(error => error.ErrorMessage)));
    }

    private async Task<Result<string>> CallLoginApi(LoginCommand loginCommand)
    {
        var loginResult =  await _invoiceApiService.Login(loginCommand);
        return loginResult;
    }

    private void SavePreferences()
    {
        if (Preferences.ContainsKey(nameof(ServerAddress)))
        {
            Preferences.Remove(nameof(ServerAddress));
        }

        if (Preferences.ContainsKey(nameof(Location)))
        {
            Preferences.Remove(nameof(Location));
        }

        Preferences.Set(nameof(ServerAddress), ServerAddress);
        Preferences.Set(nameof(Location), LocationId);
    }
}
