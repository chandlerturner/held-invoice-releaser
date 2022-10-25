using HeldInvoiceReleaser.Maui.Models.ViewModels.Startup;

namespace HeldInvoiceReleaser.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
        BindingContext = new AppShellViewModel();
    }
}
