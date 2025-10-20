using System.Linq;

namespace TARgv24.Models;

public class Board
{
    public const int Size = 3;
    public IReadOnlyList<BoardCell> Cells => _cells;
    private readonly BoardCell[] _cells;

    public Board() =>
        _cells = Enumerable.Range(0, Size * Size).Select(i => new BoardCell(i)).ToArray();

    public void Reset()
    {
        foreach (var c in _cells) c.Reset();
    }

    public bool TryPlace(int index, char symbol)
    {
        if (index < 0 || index >= _cells.Length) return false;
        return _cells[index].Place(symbol);
    }

    public (bool win, int[] line) CheckWin()
    {
        int[][] lines =
        {
            new[] {0,1,2}, new[] {3,4,5}, new[] {6,7,8},
            new[] {0,3,6}, new[] {1,4,7}, new[] {2,5,8},
            new[] {0,4,8}, new[] {2,4,6}
        };
        foreach (var line in lines)
        {
            var a = _cells[line[0]].Symbol;
            if (a is null) continue;
            if (_cells[line[1]].Symbol == a && _cells[line[2]].Symbol == a)
                return (true, line);
        }
        return (false, Array.Empty<int>());
    }

    public bool IsFull() => _cells.All(c => !c.IsEmpty);
}
