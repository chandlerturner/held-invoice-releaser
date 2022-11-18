namespace HeldInvoiceReleaser.Maui.Models.Commands;
public class ReleaseHeldInvoiceCommand
{
    public string ServerAddress { get; set; }
    public string LocationId { get; set; }
    public string PickTicket { get; set; }
}
