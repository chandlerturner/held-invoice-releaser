using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeldInvoiceReleaser.Maui.Models.ViewModels;

namespace HeldInvoiceReleaser.Maui.Pages.Startup;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;
    }
}