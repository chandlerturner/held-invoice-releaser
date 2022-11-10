using HeldInvoiceReleaser.Maui.Pages.Startup;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels.Startup;

public class LoadingPageViewModel
{
    public async void Initialize()
    {
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}

