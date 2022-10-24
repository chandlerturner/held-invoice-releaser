using HeldInvoiceReleaser.Maui.Models.ViewModels;

namespace HeldInvoiceReleaser.Maui.Pages.Startup;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}