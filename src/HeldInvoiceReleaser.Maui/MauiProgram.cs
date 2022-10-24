using HeldInvoiceReleaser.Maui.Models.ViewModels;
using HeldInvoiceReleaser.Maui.Pages.Startup;

namespace HeldInvoiceReleaser.Maui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddTransient<LoginPage>();

        builder.Services.AddTransient<LoadingPageViewModel>();

		return builder.Build();
	}
}
