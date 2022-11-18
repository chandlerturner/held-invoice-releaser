using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Models.Queries;
using HeldInvoiceReleaser.Maui.Services;
using HeldInvoiceReleaser.Models;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly IInvoiceApiService _invoiceApiService;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(NotGettingOrders))]
    bool _gettingOrders;

    public bool NotGettingOrders => !GettingOrders;
    public ObservableCollection<HeldInvoice> Orders { get; } = new();

    private string Location => Preferences.Get("Location", "");
    private string ServerAddress => Preferences.Get("ServerAddress", "");

    public MainPageViewModel(IInvoiceApiService invoiceApiService)
    {
        _invoiceApiService = invoiceApiService;
    }


    [RelayCommand]
    public async Task ReleaseWithConfirmationAsync(HeldInvoice invoice)
    {
        var confirmed = await Shell.Current.DisplayAlert("Release Invoice?",
            $"Release invoice with pick ticket {invoice.PickTicket}?", "Yes", "No");
        if (confirmed)
        {
            var command = new ReleaseHeldInvoiceCommand() { ServerAddress = ServerAddress, LocationId = Location, PickTicket = invoice.PickTicket };
            var result = await _invoiceApiService.ReleaseHeldInvoice(command);
            await GetOrdersAsync();
        }
    }

    [RelayCommand]
    public async Task GetOrdersAsync()
    {
        if (GettingOrders) return;

        try
        {
            GettingOrders = true;
            if (Orders.Count != 0) Orders.Clear();
            var query = new GetHeldInvoicesByLocationQuery() { Location = Location, ServerAddress = ServerAddress };
            var heldInvoicesByLocationResult = await _invoiceApiService.GetHeldInvoicesByLocation(query);
            if (heldInvoicesByLocationResult.IsSuccess)
            {
                var heldInvoices = heldInvoicesByLocationResult.Value;
                foreach (var order in heldInvoices)
                {
                    Orders.Add(order);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Yikes", heldInvoicesByLocationResult.Error, "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Yikes", ex.Message, "OK");
        }
        finally
        {
            GettingOrders = false;
        }
    }
}