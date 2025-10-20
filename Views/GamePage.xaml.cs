using Microsoft.Maui.Controls;
using TARgv24.ViewModels;

namespace TARgv24.Views;

public partial class GamePage : ContentPage
{
    public GamePage()
    {
        InitializeComponent();
        BindingContext = new GameViewModel(); // без DI, просто и надёжно
    }

    private void OnDragStarting(object sender, DragStartingEventArgs e)
    {
        if (BindingContext is not GameViewModel vm) return;
        e.Data.Text = vm.Model.Current.Symbol.ToString();
    }

    private void Cell_DragOver(object sender, DragEventArgs e)
    {
        e.AcceptedOperation = e.Data.Text is { Length: 1 }
            ? DataPackageOperation.Copy
            : DataPackageOperation.None;
    }

    private async void Cell_Drop(object sender, DropEventArgs e)
    {
        if (BindingContext is not GameViewModel vm) return;

        if (sender is not Grid grid || grid.Parent is not Border border || border.Parent is not Grid)
            return;

        int row = Grid.GetRow(border);
        int col = Grid.GetColumn(border);
        int index = row * 3 + col;

        var dragged = await e.Data.GetTextAsync();
        if (string.IsNullOrWhiteSpace(dragged) || dragged[0] != vm.Model.Current.Symbol)
        {
            await this.DisplayAlert("Ход", "Сейчас ходит другой символ", "OK");
            return;
        }

        vm.DropToCellCommand.Execute(index);

        await border.ScaleTo(1.08, 80);
        await border.ScaleTo(1.0, 80);
    }

}
