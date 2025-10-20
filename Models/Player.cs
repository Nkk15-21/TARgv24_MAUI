namespace TARgv24.Models;

public class Player
{
    public string Name { get; }
    public char Symbol { get; }   // 'X' or 'O'

    public Player(string name, char symbol)
    {
        Name = name;
        Symbol = symbol;
    }
}
