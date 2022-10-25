using HeldInvoiceReleaser.Maui.Models.ViewModels;

namespace HeldInvoiceReleaser.Maui.Pages;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}