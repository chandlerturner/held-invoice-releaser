using CSharpFunctionalExtensions;
using FluentValidation;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Pages;
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
        var flyoutItem = new FlyoutItem()
        {
            Title = "Release Held Invoices",
            Route = nameof(MainPage),
            FlyoutDisplayOptions = FlyoutDisplayOptions.AsSingleItem,
            Items =
            {
                new ShellContent
                {
                    Icon = Icons.Main,
                    Title = "Release Held Invoices",
                    ContentTemplate = new DataTemplate(typeof(MainPage)),
                }
            }
        };

        if (!Shell.Current.Items.Contains(flyoutItem))
        {
            Shell.Current.Items.Add(flyoutItem);
        }

        // TODO: Fix bug when loading after sign-out
        await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
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
        Preferences.Set(nameof(Location), Location);
    }
}
