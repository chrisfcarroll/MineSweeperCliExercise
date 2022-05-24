using System;
using System.Runtime.CompilerServices;

namespace MineSweeperCli;

public record struct Position(int X, int Y)
{
    public override string ToString() => $"{(char)(64+Y)}{X}";
    public Position Add(int addX, int addY) { return new Position(X + addX, Y + addY); }
    
}