using System;
using System.Collections.Generic;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace TARgv24;

public partial class StartPage : ContentPage
{
    // Один список с элементами меню: текст + фабрика создания страницы.
    private readonly List<(string Text, Func<Page> Create)> _items = new()
    {
        ("Tee lahti leht tekstiga", () => new TekstPage()),
        ("Tee lahti Figure leht",   () => new FigurePage()),
        ("Käivita taimeri",         () => new TimerPage()),
        ("Kuupäevad ja kellaajad",  () => new DateTimePage()),
        ("Viska",                   () => new NäidisPage_lumi()),
        //    ДОБАВИЛ светофор
        ("Ava Valgusfoor",          () => new ValgusfoorPage()),
    };

    public StartPage()
    {
        InitializeComponent();
        Title = "Avaleht";

        var vsl = new VerticalStackLayout
        {
            BackgroundColor = Color.FromRgb(120, 30, 50),
            Padding = new Thickness(24),
            Spacing = 12
        };

        for (int i = 0; i < _items.Count; i++)
        {
            var (text, _) = _items[i];
            var btn = new Button
            {
                Text = text,
                FontSize = 20,
                BackgroundColor = Color.FromRgb(200, 200, 100),
                TextColor = Colors.Black,
                CornerRadius = 20,
                FontFamily = "Lovin Kites 400",
                CommandParameter = i // ← вместо ZIndex
            };
            btn.Clicked += MenuButton_Clicked;
            vsl.Add(btn);
        }

        Content = new ScrollView { Content = vsl };
    }

    private async void OpenValgusfoorClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(ValgusfoorPage));
        await Shell.Current.GoToAsync("lumememm");

    }

    private async void MenuButton_Clicked(object? sender, EventArgs e)
    {
        if (sender is not Button b || b.CommandParameter is not int index) return;

        // Создаём новую страницу по фабрике и пушим в стек навигации
        var page = _items[index].Create();

        await Navigation.PushAsync(page);

    }
}
