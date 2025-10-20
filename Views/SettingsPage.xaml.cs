using Microsoft.Maui.Controls;
using TARgv24.Services;

namespace TARgv24.Views;

public partial class SettingsPage : ContentPage
{
    private readonly ThemeService _theme = new();

    public SettingsPage()
    {
        InitializeComponent();
    }

    private void Light_Clicked(object sender, EventArgs e) => _theme.Apply(AppThemeName.Light);
    private void Dark_Clicked(object sender, EventArgs e) => _theme.Apply(AppThemeName.Dark);
    private void Blue_Clicked(object sender, EventArgs e) => _theme.Apply(AppThemeName.Blue);
}
