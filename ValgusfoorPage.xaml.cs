using System;
using System.Threading;
using System.Threading.Tasks;

namespace YourNamespace;

public partial class ValgusfoorPage : ContentPage
{
    enum Light { None, Red, Yellow, Green }

    bool _isOn;               // включён ли светофор (foor on sees)
    bool _nightMode;          // öörežiim (мигает жёлтый)
    bool _autoMode;           // automaatrežiim (авто-смена)
    Light _current = Light.None;
    CancellationTokenSource? _cts; // для остановки фоновых циклов (night/auto)

    // Утилита цветов из ресурсов
    Color OffGray => (Color)Resources["OffGray"];
    Color RedOn => (Color)Resources["RedOn"];
    Color YellowOn => (Color)Resources["YellowOn"];
    Color GreenOn => (Color)Resources["GreenOn"];

    public ValgusfoorPage()
    {
        InitializeComponent();
        SetLights(Light.None);          // все серые
        SetTapsEnabled(false);          // клики по кругам запрещены, пока выключено
    }

    /* -------------------- КНОПКИ Sisse / Välja -------------------- */

    private void OnClicked(object sender, EventArgs e)
    {
        _isOn = true;
        StopAllModes();
        statusLabel.Text = "Vali valgus";   // по ТЗ: сначала “Vali valgus”
        SetTapsEnabled(true);
        // Небольшая анимация на включении
        this.ScaleTo(1.02, 150).ContinueWith(_ => this.ScaleTo(1.0, 100));
    }

    private void OffClicked(object sender, EventArgs e)
    {
        _isOn = false;
        StopAllModes();
        statusLabel.Text = "Lülita esmalt foor sisse";
        SetLights(Light.None);          // все серые
        SetTapsEnabled(false);
    }

    /* -------------------- КЛИКИ ПО КРУГАМ -------------------- */

    private async void RedTapped(object? sender, TappedEventArgs e)
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Red);
        statusLabel.Text = "Seisa";
    }

    private async void YellowTapped(object? sender, TappedEventArgs e)
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Yellow);
        statusLabel.Text = "Valmista";
    }

    private async void GreenTapped(object? sender, TappedEventArgs e)
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Green);
        statusLabel.Text = "Sõida";
    }

    bool CanTap() => _isOn && !_nightMode && !_autoMode;

    /* Маленькая анимация “нажатия” круга */
    Task Bounce(VisualElement? el) =>
        (el ?? this).ScaleTo(1.08, 120).ContinueWith(_ => (el ?? this).ScaleTo(1.0, 120));

    /* -------------------- ÖÖREŽIIM (мигает жёлтый) -------------------- */

    private async void NightClicked(object sender, EventArgs e)
    {
        if (_nightMode)
        {
            // Выключаем
            _nightMode = false;
            StopAllModes();
            statusLabel.Text = _isOn ? "Vali valgus" : "Lülita esmalt foor sisse";
            return;
        }

        // Включаем night mode
        _nightMode = true;
        _autoMode = false;
        _cts?.Cancel();
        _cts = new CancellationTokenSource();

        SetTapsEnabled(false); // роняем тапы, чтобы не мешали
        statusLabel.Text = "Öörežiim: kollane vilgub";

        try
        {
            while (_nightMode && !_cts.IsCancellationRequested)
            {
                SetLights(Light.Yellow);          // включить жёлтый
                await Task.WhenAll(
                    this.FadeTo(0.95, 200),       // лёгкое затемнение страницы
                    yellowBox.ScaleTo(1.06, 200)
                );
                await Task.Delay(500, _cts.Token);

                // погасить всё
                SetLights(Light.None);
                await Task.WhenAll(
                    this.FadeTo(1.0, 200),
                    yellowBox.ScaleTo(1.0, 200)
                );
                await Task.Delay(500, _cts.Token);
            }
        }
        catch (TaskCanceledException) { /* ок */ }
        finally
        {
            if (!_nightMode) return;
        }
    }

    /* -------------------- AUTOMaatrežiim (каждые 2 сек) -------------------- */

    private async void AutoClicked(object sender, EventArgs e)
    {
        if (!_isOn)
        {
            statusLabel.Text = "Lülita esmalt foor sisse";
            return;
        }

        if (_autoMode)
        {
            _autoMode = false;
            StopAllModes();
            statusLabel.Text = "Vali valgus";
            SetTapsEnabled(true);
            return;
        }

        _autoMode = true;
        _nightMode = false;
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        SetTapsEnabled(false);

        var sequence = new[] { Light.Red, Light.Green, Light.Yellow }; // классический цикл
        int i = 0;
        try
        {
            while (_autoMode && !_cts.IsCancellationRequested)
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
        catch (TaskCanceledException) { /* ок */ }
    }

    /* -------------------- СЛУЖЕБНОЕ -------------------- */

    void SetLights(Light active)
    {
        _current = active;

        redBox.BackgroundColor = active == Light.Red ? RedOn : OffGray;
        yellowBox.BackgroundColor = active == Light.Yellow ? YellowOn : OffGray;
        greenBox.BackgroundColor = active == Light.Green ? GreenOn : OffGray;
    }

    void SetTapsEnabled(bool enabled)
    {
        // InputTransparent = true => элемент НЕ получает касания
        redBox.InputTransparent = yellowBox.InputTransparent = greenBox.InputTransparent = !enabled;
    }

    void StopAllModes()
    {
        _nightMode = false;
        _autoMode = false;
        _cts?.Cancel();
        _cts = null;
        SetTapsEnabled(_isOn);
        this.Opacity = 1.0;
        yellowBox.Scale = 1.0;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _cts?.Cancel(); // на всякий случай гасим фоновые циклы
    }
}
