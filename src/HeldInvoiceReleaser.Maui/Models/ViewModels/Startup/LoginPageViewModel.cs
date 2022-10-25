using CSharpFunctionalExtensions;
using FluentValidation;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Services;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;

public partial class LoginPageViewModel : BaseViewModel
{
    private readonly IInvoiceApiService _invoiceApiService;
    private readonly IValidator<LoginCommand> _loginCommandValidator;
    [ObservableProperty] private string _server;

    [ObservableProperty] private string _location;
    [ObservableProperty] private string _loginError;

    public LoginPageViewModel(IInvoiceApiService invoiceApiService, IValidator<LoginCommand> loginCommandValidator)
    {
        _invoiceApiService = invoiceApiService;
        _loginCommandValidator = loginCommandValidator;

        Server = Preferences.Get(nameof(Server), string.Empty);
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
        LoginCommand loginCommand = new() { Server = Server, Location = Location };
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
        if (Preferences.ContainsKey(nameof(Server)))
        {
            Preferences.Remove(nameof(Server));
        }

        if (Preferences.ContainsKey(nameof(Location)))
        {
            Preferences.Remove(nameof(Location));
        }

        Preferences.Set(nameof(Server), Server);
        Preferences.Set(nameof(Location), Location);
    }
}
