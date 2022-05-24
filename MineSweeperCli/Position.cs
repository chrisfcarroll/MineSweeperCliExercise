namespace MineSweeperCli;

public record struct Position(int X, int Y)
{
    public override string ToString() => $"{(char)(48+Y)}{X}";
}