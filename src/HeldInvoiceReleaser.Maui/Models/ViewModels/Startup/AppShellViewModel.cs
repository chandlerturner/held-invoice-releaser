using CommunityToolkit.Mvvm.Input;
using HeldInvoiceReleaser.Maui.Pages.Startup;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels.Startup;

public partial class AppShellViewModel : BaseViewModel
{
    [RelayCommand]
    async void SignOut()
    {
        //if (Preferences.ContainsKey(nameof(App.UserDetails)))
        //{
        //    Preferences.Remove(nameof(App.UserDetails));
        //}
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}

