using HeldInvoiceReleaser.Maui.Models.ViewModels.Startup;

namespace HeldInvoiceReleaser.Maui.Pages.Startup;

public partial class LoadingPage : ContentPage
{
    private readonly LoadingPageViewModel _viewModel;
    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();

        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Initialize();
    }
}