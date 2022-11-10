using FluentValidation;
using HeldInvoiceReleaser.Maui.Models.Commands;
using HeldInvoiceReleaser.Maui.Models.ViewModels;
using HeldInvoiceReleaser.Maui.Models.ViewModels.Startup;
using HeldInvoiceReleaser.Maui.Pages;
using HeldInvoiceReleaser.Maui.Pages.Startup;
using HeldInvoiceReleaser.Maui.Services;

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

                fonts.AddFont("Lato/Lato-Regular.ttf", "LatoRegular");
                fonts.AddFont("Lato/Lato-Bold.ttf", "LatoBold");
                fonts.AddFont("Lato/Lato-Black.ttf", "LatoBlack");
            });

        builder.Services.AddHttpClient();


        builder.Services.AddTransient<LoadingPage>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainPage>();

        builder.Services.AddTransient<LoadingPageViewModel>();
        builder.Services.AddTransient<LoginPageViewModel>();
        builder.Services.AddTransient<MainPageViewModel>();

        builder.Services.AddSingleton<IRoutingService, ShellRoutingService>();
        builder.Services.AddSingleton<IInvoiceApiService, InvoiceApiService>();
        builder.Services.AddTransient<IValidator<LoginCommand>, LoginCommandValidator>();

		return builder.Build();
	}
}
