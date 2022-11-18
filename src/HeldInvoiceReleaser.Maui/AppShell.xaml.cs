using HeldInvoiceReleaser.Maui.Models.ViewModels.Startup;
using HeldInvoiceReleaser.Maui.Pages;
using HeldInvoiceReleaser.Maui.Pages.Startup;

namespace HeldInvoiceReleaser.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

        BindingContext = new AppShellViewModel();
    }
}
