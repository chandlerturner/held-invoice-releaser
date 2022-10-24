using Microsoft.Toolkit.Mvvm.Input;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;

public partial class LoginPageViewModel
{
    public string Server { get; set; }
    public string Location { get; set; }


    [ICommand]
    async void Login()
    {
        await Task.Delay(TimeSpan.FromSeconds(1));
    }
}
