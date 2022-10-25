using Microsoft.Toolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using HeldInvoiceReleaser.Models;
using Microsoft.Toolkit.Mvvm.Input;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    private readonly IConnectivity _connectivity;
    [ObservableProperty]
    [AlsoNotifyChangeFor(nameof(NotGettingOrders))]
    bool gettingOrders;

    public bool NotGettingOrders => !GettingOrders;
    public ObservableCollection<HeldInvoice> Orders { get; } = new();

    public MainPageViewModel(IConnectivity connectivity)
    {
        _connectivity = connectivity;
    }


    [ICommand]
    public async Task ReleaseWithConfirmationAsync(HeldInvoice order)
    {
        var confirmed = await Shell.Current.DisplayAlert("Release Order?", $"Are you sure you want to release pick ticket {order.PickTicketNumber}?", "Yes", "No");
        if (confirmed)
        {
            //await _orderRepo.ReleaseOrder(order);
            await GetOrdersAsync();
        }
    }

    [ICommand]
    async Task GetOrdersAsync()
    {
        if (GettingOrders) return;

        try
        {
            GettingOrders = true;
            await Task.Delay(TimeSpan.FromSeconds(3));
            if (Orders.Count != 0) Orders.Clear();
            var ordersFromRepoResult = Result.Failure<IEnumerable<HeldInvoice>>("get it together"); //await _orderRepo.GetAllReleasableOrders();
            if (ordersFromRepoResult.IsSuccess)
            {
                var ordersFromRepo = ordersFromRepoResult.Value;
                foreach (var order in ordersFromRepo.Take(11))
                {
                    Orders.Add(order);
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Yikes", ordersFromRepoResult.Error, "OK");
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
