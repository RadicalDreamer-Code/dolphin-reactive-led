using Microsoft.AspNetCore.Components.WebView.Maui;
using ww_led_control.Data;
using ww_led_control.Services;

namespace ww_led_control;

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
			});

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
#endif
		
		builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddSingleton<SerialManager>();
		builder.Services.AddSingleton<Dolphin>(s =>
		{
			var serialManager = s.GetRequiredService<SerialManager>();
			return new Dolphin(serialManager);
		});

        return builder.Build();
	}
}
