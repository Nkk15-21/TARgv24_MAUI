using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using TARgv24.Models;
using TARgv24.Services;

namespace TARgv24.ViewModels;

public partial class GameViewModel : INotifyPropertyChanged
{
    private readonly ScoreService _scores = new();
    public Game Model { get; } = new();

    public ObservableCollection<BoardCell> Cells { get; } = new();

    public char CurrentSymbol => Model.Current.Symbol;
    public string CurrentPlayerText => $"Ходит: {Model.Current.Name} ({Model.Current.Symbol})";
    public string MovesText => $"Ходы: {Model.Moves}";
    public string TimerText => $"Время: {Elapsed:mm':'ss}";

    private readonly IDispatcherTimer _timer;
    private TimeSpan Elapsed => (Model.EndTime ?? DateTimeOffset.Now) - Model.StartTime;

    public ICommand DropToCellCommand { get; }
    public ICommand ResetCommand { get; }

    public event PropertyChangedEventHandler? PropertyChanged;

    public GameViewModel()
    {
        foreach (var c in Model.Board.Cells) Cells.Add(c);

        DropToCellCommand = new Command<int>(OnDropToCell);
        ResetCommand = new Command(Restart);

        _timer = Application.Current!.Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromSeconds(1);
        _timer.Tick += (_, __) => OnPropertyChanged(nameof(TimerText));
        _timer.Start();
    }

    private async void OnDropToCell(int cellIndex)
    {
        if (Model.Finished) return;

        var (moved, result) = Model.TryMove(cellIndex);
        if (!moved)
        {
            await Shell.Current.CurrentPage.DisplayAlert("Нельзя", "Клетка занята", "OK");
            return;
        }

        RefreshBindings();

        if (result is not null)
        {
            _scores.Add(result);
            var msg = result.HasWinner
                ? $"Победил: {result.WinnerName} за {result.Duration:mm':'ss}, ходов: {result.Moves}"
                : $"Ничья за {result.Duration:mm':'ss}, ходов: {result.Moves}";
            await Shell.Current!.CurrentPage.DisplayAlert("Игра окончена", msg, "OK");
        }
    }

    public void Restart()
    {
        Model.Restart();
        RefreshBindings();
    }

    private void RefreshBindings()
    {
        OnPropertyChanged(nameof(CurrentSymbol));
        OnPropertyChanged(nameof(CurrentPlayerText));
        OnPropertyChanged(nameof(MovesText));
        OnPropertyChanged(nameof(TimerText));
    }

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
