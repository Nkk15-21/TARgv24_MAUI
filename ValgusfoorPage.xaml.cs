using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace TARgv24;

public partial class ValgusfoorPage : ContentPage
{
    private enum Light { None, Red, Yellow, Green }

    private bool _isOn;
    private bool _nightMode;
    private bool _autoMode;
    private Light _current = Light.None;
    private CancellationTokenSource? _cts;

    private Color OffGray => (Color)Resources["OffGray"];
    private Color RedOn => (Color)Resources["RedOn"];
    private Color YellowOn => (Color)Resources["YellowOn"];
    private Color GreenOn => (Color)Resources["GreenOn"];

    public ValgusfoorPage()
    {
        InitializeComponent();
        SetLights(Light.None);
        SetTapsEnabled(false);
    }

    private async void OnClicked(object sender, EventArgs e)
    {
        _isOn = true;
        StopAllModes();
        statusLabel.Text = "Vali valgus";
        SetTapsEnabled(true);
        await this.ScaleTo(1.02, 150);
        await this.ScaleTo(1.0, 100);
    }

    private void OffClicked(object sender, EventArgs e)
    {
        _isOn = false;
        StopAllModes();
        statusLabel.Text = "Lülita esmalt foor sisse";
        SetLights(Light.None);
        SetTapsEnabled(false);
    }

    private async void RedTapped(object sender, TappedEventArgs e)
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Red);
        statusLabel.Text = "Seisa";
    }

    private async void YellowTapped(object sender, TappedEventArgs e)
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Yellow);
        statusLabel.Text = "Valmista";
    }

    private async void GreenTapped(object sender, TappedEventArgs e)
    {
        if (!CanTap()) return;
        await Bounce(sender as VisualElement);
        SetLights(Light.Green);
        statusLabel.Text = "Sõida";
    }

    private bool CanTap() => _isOn && !_nightMode && !_autoMode;

    private async Task Bounce(VisualElement? el)
    {
        var v = el ?? this;
        await v.ScaleTo(1.08, 120);
        await v.ScaleTo(1.0, 120);
    }

    private async void NightClicked(object sender, EventArgs e)
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
        RestartCts();
        SetTapsEnabled(false);

        var sequence = new[] { Light.Red, Light.Green, Light.Yellow };
        var i = 0;

        try
        {
            while (_autoMode && !_cts!.IsCancellationRequested)
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

    private void SetLights(Light active)
    {
        _current = active;
        redBox.BackgroundColor = active == Light.Red ? RedOn : OffGray;
        yellowBox.BackgroundColor = active == Light.Yellow ? YellowOn : OffGray;
        greenBox.BackgroundColor = active == Light.Green ? GreenOn : OffGray;
    }

    private void SetTapsEnabled(bool enabled)
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
