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

    public LumememmPage() // конструктор страницы
    {
        InitializeComponent();

        // Берём элементы по имени из XAML (надёжно, без автогенерации полей)
        _root = this.FindByName<Grid>("SnowmanRoot");// корневой элемент снеговика
        _head = this.FindByName<Ellipse>("Head");// голова
        _body = this.FindByName<Ellipse>("Body");
        _hatTop = this.FindByName<BoxView>("HatTop");
        _hatBrim = this.FindByName<BoxView>("HatBrim");
        _nose = this.FindByName<BoxView>("Nose");
        _armLeft = this.FindByName<BoxView>("ArmLeft");
        _armRight = this.FindByName<BoxView>("ArmRight");
    }

    // ==== Утилиты для работы с группой ====

    void SetGroupOpacity(double v)// установка прозрачности всей группы
    {
        if (_root == null) return;// защита от ошибок
        _ = _root.FadeTo(v, 1);// мгновенно устанавливаем прозрачность
    }
      
    void SetGroupIsVisible(bool vis)// установка видимости всей группы
    {
        if (_root == null) return;
        _root.IsVisible = vis;// мгновенно устанавливаем видимость
    }

    async Task DanceAsync()// анимация танца
    {
        if (_root == null) return;
        uint d = (uint)(220 / _speed);// длительность одного движения
        await _root.TranslateTo(16, 0, d);// вправо
        await _root.TranslateTo(-16, 0, d);// влево
        await _root.TranslateTo(0, 0, d);// в центр
        await _root.RotateTo(8, d);// наклон вправо
        await _root.RotateTo(-8, d);// наклон влево
        await _root.RotateTo(0, d);// выпрямление
        await _root.TranslateTo(0, -10, d);// вверх
        await _root.TranslateTo(0, 0, d);// вниз
    }

    async Task MeltAsync()// анимация таяния
    {
        if (_root == null) return;
        await Task.WhenAll(// выполняем анимации параллельно
            _root.ScaleTo(0.75, (uint)(600 / _speed)),// сжимаем
            _root.FadeTo(0, (uint)(700 / _speed))// делаем прозрачным
        );
    }

    // ==== Обработчики ====

    void OnOpacityChanged(object sender, ValueChangedEventArgs e) => SetGroupOpacity(e.NewValue);// обработчик изменения прозрачности

    void OnSpeedChanged(object sender, ValueChangedEventArgs e) => _speed = e.NewValue;// обработчик изменения скорости

    async void OnActionClicked(object sender, EventArgs e)// обработчик нажатия кнопки действия
    {
        var action = ActionPicker.SelectedItem as string ?? "";// получаем выбранное действие

        switch (action)// выполняем действие
        {
            case "Peida":// спрятать
                SetGroupIsVisible(false);// мгновенно скрываем
                break;

            case "Näita":// показать
                SetGroupIsVisible(true);
                await _root.FadeTo(1, 150);// плавно показываем
                await _root.ScaleTo(1, 150);// плавно масштабируем
                break;

            case "Muuda värvi":// изменить цвет
                {
                    var rnd = new Random();
                    var c = Color.FromRgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));// случайный цвет

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
