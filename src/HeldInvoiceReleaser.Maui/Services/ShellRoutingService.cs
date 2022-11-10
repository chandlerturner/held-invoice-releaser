namespace HeldInvoiceReleaser.Maui.Services;

public class ShellRoutingService : IRoutingService
{
    public Task NavigateTo(string route)
    {
        return Shell.Current.GoToAsync(route);
    }

    public Task GoBack()
    {
        return Shell.Current.Navigation.PopAsync();
    }

    public Task GoBackModal()
    {
        return Shell.Current.Navigation.PopModalAsync();
    }
}