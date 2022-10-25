using CSharpFunctionalExtensions;
using FluentValidation;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    [ObservableProperty] private string _serverAddress;
    [ObservableProperty] private string _location;
    [ObservableProperty] private string _loginError;

    private readonly IInvoiceApiService _invoiceApiService;
    private readonly IValidator<LoginCommand> _loginCommandValidator;

    public LoginPageViewModel(IInvoiceApiService invoiceApiService, IValidator<LoginCommand> loginCommandValidator)
    {
        _invoiceApiService = invoiceApiService;
        _loginCommandValidator = loginCommandValidator;

        ServerAddress = Preferences.Get(nameof(ServerAddress), string.Empty);
        Location = Preferences.Get(nameof(Location), string.Empty);
    }

    [ICommand]
    async void Login()
    {
        LoginError = string.Empty;
        var loginResult = await CreateCommand()
            .Bind(CallLoginApi)
            .TapError(error => LoginError = error)
            .Tap(SavePreferences);
    }

    private Result<LoginCommand> CreateCommand()
    {
        LoginCommand loginCommand = new() { ServerAddress = ServerAddress, Location = Location };
        var validationResult = _loginCommandValidator.Validate(loginCommand);
        return validationResult.IsValid
            ? loginCommand
            : Result.Failure<LoginCommand>(
                string.Join('|', validationResult.Errors.Select(error => error.ErrorMessage)));
    }

    private async Task<Result> CallLoginApi(LoginCommand loginCommand)
    {
        return await _invoiceApiService.Login(loginCommand);
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
        Preferences.Set(nameof(Location), Location);
    }
}
