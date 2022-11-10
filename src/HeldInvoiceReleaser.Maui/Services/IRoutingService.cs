namespace HeldInvoiceReleaser.Maui.Services;

public interface IRoutingService
{
    Task GoBack();
    Task GoBackModal();
    Task NavigateTo(string route);
}