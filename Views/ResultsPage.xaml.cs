using Microsoft.Maui.Controls;
using TARgv24.Services;

namespace TARgv24.Views;

public partial class ResultsPage : ContentPage
{
    private readonly ScoreService _scores = new();

    public ResultsPage()
    {
        InitializeComponent();
        Appearing += (_, __) => Refresh();
    }

    private void Refresh()
    {
        var list = _scores.LoadAll();
        List.ItemsSource = list;
    }
}
