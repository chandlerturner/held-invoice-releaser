using HeldInvoiceReleaser.Maui.Pages.Startup;
using Microsoft.Toolkit.Mvvm.Input;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels.Startup;

public partial class AppShellViewModel : BaseViewModel
{
    [ICommand]
    async void SignOut()
    {
        //if (Preferences.ContainsKey(nameof(App.UserDetails)))
        //{
        //    Preferences.Remove(nameof(App.UserDetails));
        //}
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
}

