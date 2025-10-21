using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace TARgv24;

public partial class ValgusfoorPage : ContentPage // светофор
{
    private enum Light { None, Red, Yellow, Green }// типы света

    private bool _isOn;// включен ли светофор
    private bool _nightMode;// ночной режим
    private bool _autoMode;// автоматический режим
    private Light _current = Light.None;// текущий свет
    private CancellationTokenSource? _cts;// токен отмены для асинхронных операций

    private Color OffGray => (Color)Resources["OffGray"];// цвет выключенного света
    private Color RedOn => (Color)Resources["RedOn"];// цвет включенного красного света
    private Color YellowOn => (Color)Resources["YellowOn"];// цвет включенного желтого света
    private Color GreenOn => (Color)Resources["GreenOn"];// цвет включенного зеленого света

    public ValgusfoorPage()// конструктор страницы
    {
        InitializeComponent();// инициализация компонентов
        SetLights(Light.None);// выключаем все огни
        SetTapsEnabled(false);// отключаем нажатия на огни
    }

    private async void OnClicked(object sender, EventArgs e)// обработчик нажатия кнопки включения
    {
        _isOn = true;// устанавливаем флаг включения
        StopAllModes();// останавливаем все режимы
        statusLabel.Text = "Vali valgus";
        SetTapsEnabled(true);// включаем нажатия на огни
        await this.ScaleTo(1.02, 150);// анимация нажатия
        await this.ScaleTo(1.0, 100);// анимация отпускания
    }

    private void OffClicked(object sender, EventArgs e)// обработчик нажатия кнопки выключения
    {
        _isOn = false;
        StopAllModes();
        statusLabel.Text = "Lülita esmalt foor sisse";// обновляем статус
        SetLights(Light.None);// выключаем все огни
        SetTapsEnabled(false);// отключаем нажатия на огни
    }

    private async void RedTapped(object sender, TappedEventArgs e)// обработчик нажатия красного огня
    {
        if (!CanTap()) return;// проверяем, можно ли нажимать
        await Bounce(sender as VisualElement);// анимация нажатия
        SetLights(Light.Red);// устанавливаем красный свет
        statusLabel.Text = "Seisa";
    }

    private async void YellowTapped(object sender, TappedEventArgs e)// обработчик нажатия желтого огня
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Yellow);
        statusLabel.Text = "Valmista";
    }

    private async void GreenTapped(object sender, TappedEventArgs e)// обработчик нажатия зеленого огня
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Green);
        statusLabel.Text = "Sõida";
    }

    private bool CanTap() => _isOn && !_nightMode && !_autoMode; // проверяем, можно ли нажимать огни

    private async Task Bounce(VisualElement? el)// анимация нажатия
    {
        var v = el ?? this;// если элемент не передан, используем текущий
        await v.ScaleTo(1.08, 120);
        await v.ScaleTo(1.0, 120);
    }

    private async void NightClicked(object sender, EventArgs e)// обработчик нажатия кнопки ночного режима
    {
        if (!_isOn)// проверяем, включен ли светофор
        {
            statusLabel.Text = "Lülita esmalt foor sisse";
            return;
        }
        if (_nightMode)// если уже в ночном режиме, выключаем его
        {
            if (_nightMode)
            {
                _nightMode = false;
                StopAllModes();
                statusLabel.Text = _isOn ? "Vali valgus" : "Lülita esmalt foor sisse";
                return;
            }

            _nightMode = true;
            _autoMode = false;
            RestartCts();

            SetTapsEnabled(false);
            statusLabel.Text = "Öörežiim: kollane vilgub";

            try
            {
                while (_nightMode && !_cts!.IsCancellationRequested)
                {
                    SetLights(Light.Yellow);
                    await Task.WhenAll(
                        this.FadeTo(0.95, 200),
                        yellowBox.ScaleTo(1.06, 200)
                    );
                    await Task.Delay(500, _cts.Token);

                    SetLights(Light.None);
                    await Task.WhenAll(
                        this.FadeTo(1.0, 200),
                        yellowBox.ScaleTo(1.0, 200)
                    );
                    await Task.Delay(500, _cts.Token);
                }
            }
            catch (TaskCanceledException)
            {
                // нормальная остановка
            }
            // НИКАКОГО return в finally — CS0157 исчезает
        }
    }  

    private async void AutoClicked(object sender, EventArgs e)// обработчик нажатия кнопки автоматического режима
    {
        if (!_isOn)// проверяем, включен ли светофор
        {
            statusLabel.Text = "Lülita esmalt foor sisse";
            return;
        }

        if (_autoMode)// если уже в автоматическом режиме, выключаем его
        {
            _autoMode = false;
            StopAllModes();
            statusLabel.Text = "Vali valgus";
            SetTapsEnabled(true);
            return;
        }

        _autoMode = true;
        _nightMode = false;
        RestartCts();
        SetTapsEnabled(false);

        var sequence = new[] { Light.Red, Light.Yellow, Light.Green, Light.Yellow, Light.Red };// последовательность огней
        var i = 0;

        try
        {
            while (_autoMode && !_cts!.IsCancellationRequested)// основной цикл автоматического режима
            {
                var next = sequence[i++ % sequence.Length];
                SetLights(next);
                statusLabel.Text = next switch
                {
                    Light.Red => "Seisa",
                    Light.Yellow => "Valmista",
                    Light.Green => "Sõida",
                    _ => "Vali valgus"
                };
                await Task.Delay(2000, _cts.Token);
            }
        }
        catch (TaskCanceledException)
        {
            // нормальная остановка
        }
    }

    private void SetLights(Light active) // установка активного света
    {
        _current = active;
        redBox.BackgroundColor = active == Light.Red ? RedOn : OffGray;
        yellowBox.BackgroundColor = active == Light.Yellow ? YellowOn : OffGray;
        greenBox.BackgroundColor = active == Light.Green ? GreenOn : OffGray;
    }

    private void SetTapsEnabled(bool enabled)// включение или отключение нажатий на огни
    {
        var block = !enabled;
        redBox.InputTransparent = block;
        yellowBox.InputTransparent = block;
        greenBox.InputTransparent = block;
    }

    private void StopAllModes()
    {
        _nightMode = false;
        _autoMode = false;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

        SetTapsEnabled(_isOn);
        this.Opacity = 1.0;
        yellowBox.Scale = 1.0;
    }

    private void RestartCts()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _cts?.Cancel();
    }
}
