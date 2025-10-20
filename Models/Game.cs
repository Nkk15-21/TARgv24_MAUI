using System;

namespace TARgv24.Models;

public class Game
{
    public Board Board { get; } = new();
    public Player P1 { get; }
    public Player P2 { get; }
    public Player Current { get; private set; }

    public int Moves { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset? EndTime { get; private set; }
    public bool Finished => EndTime is not null;

    public Game(string p1Name = "Player X", string p2Name = "Player O")
    {
        P1 = new Player(p1Name, 'X');
        P2 = new Player(p2Name, 'O');
        Current = P1;
        StartTime = DateTimeOffset.Now;
    }

    public void Restart()
    {
        Board.Reset();
        Moves = 0;
        EndTime = null;
        Current = P1;
        StartTime = DateTimeOffset.Now;
    }

    public (bool moved, GameResult? result) TryMove(int index)
    {
        if (Finished) return (false, null);
        var moved = Board.TryPlace(index, Current.Symbol);
        if (!moved) return (false, null);

        Moves++;

        var (win, line) = Board.CheckWin();
        if (win)
        {
            foreach (var i in line) Board.Cells[i].SetWinning(true);
            EndTime = DateTimeOffset.Now;
            return (true, new GameResult(Current.Name, Current.Symbol, Moves, StartTime, EndTime.Value, true));
        }

        if (Board.IsFull())
        {
            EndTime = DateTimeOffset.Now;
            return (true, new GameResult("Draw", '-', Moves, StartTime, EndTime.Value, false));
        }

        Current = Current == P1 ? P2 : P1;
        return (true, null);
    }
}

public record GameResult(string WinnerName, char WinnerSymbol, int Moves, DateTimeOffset Started, DateTimeOffset Finished, bool HasWinner)
{
    public TimeSpan Duration => Finished - Started;
}
