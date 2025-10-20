namespace TARgv24.Models;

public class BoardCell
{
    public int Index { get; }
    public char? Symbol { get; private set; }
    public bool IsWinning { get; private set; }

    public BoardCell(int index) => Index = index;

    public bool IsEmpty => !Symbol.HasValue;

    public bool Place(char symbol)
    {
        if (!IsEmpty) return false;
        Symbol = symbol;
        return true;
    }

    public void SetWinning(bool win) => IsWinning = win;
    public void Reset() { Symbol = null; IsWinning = false; }
}
