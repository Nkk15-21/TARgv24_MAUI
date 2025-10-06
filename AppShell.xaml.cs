using Microsoft.Maui.Controls;

namespace TARgv24;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        // Регистрируем маршрут для навигации на светофор
        Routing.RegisterRoute(nameof(ValgusfoorPage), typeof(ValgusfoorPage));
    }
}
