using HeldInvoiceReleaser.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace HeldInvoiceReleaser.Maui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        //Border less entry
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
        {
            if (view is BorderlessEntry)
            {
#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Colors.Transparent.ToPlatform());
#elif __IOS__
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            }
        });

        MainPage = new AppShell();
	}
}
