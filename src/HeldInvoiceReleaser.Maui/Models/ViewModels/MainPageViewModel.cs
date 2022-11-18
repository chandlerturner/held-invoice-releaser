using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
    public async Task ReleaseWithConfirmationAsync(HeldInvoice order)
    {
        var confirmed = await Shell.Current.DisplayAlert("Release Order?",
            $"Are you sure you want to release pick ticket {order.PickTicketNumber}?", "Yes", "No");
        if (confirmed)
        {
            //await _orderRepo.ReleaseOrder(order);
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