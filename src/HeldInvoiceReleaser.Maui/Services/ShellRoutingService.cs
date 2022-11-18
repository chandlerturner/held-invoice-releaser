using HeldInvoiceReleaser.Maui.Pages;

namespace HeldInvoiceReleaser.Maui.Services;

public class ShellRoutingService : IRoutingService
{
    public async Task NavigateToAsync(string route)
    {
        Task GoToPage() => Shell.Current.GoToAsync(route);
        if (MainThread.IsMainThread)
            await GoToPage();
        else
            MainThread.BeginInvokeOnMainThread(async () => await GoToPage());
    }
}