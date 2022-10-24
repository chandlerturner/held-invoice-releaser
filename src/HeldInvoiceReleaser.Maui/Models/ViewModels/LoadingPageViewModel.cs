using HeldInvoiceReleaser.Maui.Pages.Startup;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;

public class LoadingPageViewModel
{
    public LoadingPageViewModel()
    {
        NavigateToLogin();
    }
    private async void NavigateToLogin()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}

