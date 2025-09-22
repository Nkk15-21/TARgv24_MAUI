using Microsoft.Maui.Controls;

namespace TARgv24;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(ValgusfoorPage), typeof(ValgusfoorPage));
    }
}
