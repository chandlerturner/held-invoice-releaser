namespace HeldInvoiceReleaser.Models;
public class HeldInvoice
{
    public string? ShipToName { get; set; }
    public string? CustomerPurchaseOrder { get; set; }
    public string PickTicket { get; set; } = default!;
    public string? ShippingOrderNumber { get; set; }
    public string? PickedBy { get; set; }
    public string? ContactName { get; set; }
    public string? CustomerShipTo { get; set; }
    public string Location { get; set; } = default!;
}