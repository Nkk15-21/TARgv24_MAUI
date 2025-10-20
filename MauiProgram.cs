using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using Microsoft.Extensions.DependencyInjection;
using TripsTrapsTrull.Services;
using TripsTrapsTrull.ViewModels;
using TripsTrapsTrull.Views;

namespace TripsTrapsTrull;

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

        // Services
        builder.Services.AddSingleton<ThemeService>();
        builder.Services.AddSingleton<ScoreService>();

        // ViewModels
        builder.Services.AddTransient<GameViewModel>();

        // Views
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<GamePage>();
        builder.Services.AddSingleton<ResultsPage>();
        builder.Services.AddSingleton<SettingsPage>();

        return builder.Build();
    }
}
