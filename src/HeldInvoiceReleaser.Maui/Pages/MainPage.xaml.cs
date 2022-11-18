using HeldInvoiceReleaser.Maui.Models.ViewModels;

namespace HeldInvoiceReleaser.Maui.Pages;

public partial class MainPage : ContentPage
{
    private readonly MainPageViewModel _viewModel;

    public MainPage(MainPageViewModel viewModel)
    {
        _viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();
    }

    protected override async void OnAppearing() { base.OnAppearing(); await _viewModel.GetOrdersAsync(); }
}