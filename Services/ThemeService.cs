using System;
using Microsoft.Maui.Controls;

namespace TARgv24.Services;

public enum AppThemeName { Light, Dark, Blue }

public class ThemeService
{
    public AppThemeName Current { get; private set; } = AppThemeName.Light;

    public void Apply(AppThemeName theme)
    {
        Current = theme;
        var dicts = Application.Current!.Resources.MergedDictionaries;
        dicts.Clear();

        string src = theme switch
        {
            AppThemeName.Light => "Themes/LightTheme.xaml",
            AppThemeName.Dark => "Themes/DarkTheme.xaml",
            AppThemeName.Blue => "Themes/BlueTheme.xaml",
            _ => "Themes/LightTheme.xaml"
        };

        dicts.Add(new ResourceDictionary { Source = new Uri(src, UriKind.Relative) });
    }
}
