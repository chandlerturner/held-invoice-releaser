using CommunityToolkit.Mvvm.ComponentModel;

namespace HeldInvoiceReleaser.Maui.Models.ViewModels;
public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        string title;
    }
