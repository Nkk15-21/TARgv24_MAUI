using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace TARgv24;

public partial class LumememmPage : ContentPage
{
    double _speed = 1.0;

    // ЯВНО храним ссылку на корень снеговика
    Grid _root;                       // это вместо автосгенерированного SnowmanRoot
    Ellipse _head, _body;
    BoxView _hatTop, _hatBrim, _nose, _armLeft, _armRight;

    public LumememmPage()
    {
        InitializeComponent();

        // Берём элементы по имени из XAML (надёжно, без автогенерации полей)
        _root = this.FindByName<Grid>("SnowmanRoot");
        _head = this.FindByName<Ellipse>("Head");
        _body = this.FindByName<Ellipse>("Body");
        _hatTop = this.FindByName<BoxView>("HatTop");
        _hatBrim = this.FindByName<BoxView>("HatBrim");
        _nose = this.FindByName<BoxView>("Nose");
        _armLeft = this.FindByName<BoxView>("ArmLeft");
        _armRight = this.FindByName<BoxView>("ArmRight");
    }

    // ==== Утилиты для работы с группой ====

    void SetGroupOpacity(double v)
    {
        if (_root == null) return;
        _ = _root.FadeTo(v, 1);
    }
      
    void SetGroupIsVisible(bool vis)
    {
        if (_root == null) return;
        _root.IsVisible = vis;
    }

    async Task DanceAsync()
    {
        if (_root == null) return;
        uint d = (uint)(220 / _speed);
        await _root.TranslateTo(16, 0, d);
        await _root.TranslateTo(-16, 0, d);
        await _root.TranslateTo(0, 0, d);
        await _root.RotateTo(8, d);
        await _root.RotateTo(-8, d);
        await _root.RotateTo(0, d);
        await _root.TranslateTo(0, -10, d);
        await _root.TranslateTo(0, 0, d);
    }

    async Task MeltAsync()
    {
        if (_root == null) return;
        await Task.WhenAll(
            _root.ScaleTo(0.75, (uint)(600 / _speed)),
            _root.FadeTo(0, (uint)(700 / _speed))
        );
    }

    // ==== Обработчики ====

    void OnOpacityChanged(object sender, ValueChangedEventArgs e) => SetGroupOpacity(e.NewValue);

    void OnSpeedChanged(object sender, ValueChangedEventArgs e) => _speed = e.NewValue;

    async void OnActionClicked(object sender, EventArgs e)
    {
        var action = ActionPicker.SelectedItem as string ?? "";

        switch (action)
        {
            case "Peida":
                SetGroupIsVisible(false);
                break;

            case "Näita":
                SetGroupIsVisible(true);
                await _root.FadeTo(1, 150);
                await _root.ScaleTo(1, 150);
                break;

            case "Muuda värvi":
                {
                    var rnd = new Random();
                    var c = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

                    // тело немного полупрозрачное — выглядит естественнее
                    _body.Fill = new SolidColorBrush(c.WithAlpha(0.9f));

                    // шляпу чуть затемним в ту же гамму
                    var hatColor = c.WithSaturation(0.7f);
                    _hatTop.Color = hatColor;
                    _hatBrim.Color = hatColor;

                    // нос оставим «морковкой»
                    _nose.Color = Colors.OrangeRed;
                    break;
                }

            case "Sulata":
                await MeltAsync();
                break;

            case "Tantsi":
                await DanceAsync();
                break;

            default:
                await DisplayAlert("Viga", "Vali tegevus.", "OK");
                break;
        }
    }
}
