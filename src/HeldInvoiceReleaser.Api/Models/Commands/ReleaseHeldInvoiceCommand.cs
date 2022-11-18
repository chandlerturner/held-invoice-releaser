namespace HeldInvoiceReleaser.Api.Models.Commands;
public class ReleaseHeldInvoiceCommand
{
    public string PickTicket { get; set; }
    public string LocationId { get; set; }
}
