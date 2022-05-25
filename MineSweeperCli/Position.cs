using System;
using System.Runtime.CompilerServices;

namespace MineSweeperCli;

public readonly record struct Position(int X, int Y)
{
    public override string ToString() => $"{(char)(64+Y)}{X}";
    public Position Add(int addX, int addY) { return new Position(X + addX, Y + addY); }

    public bool IsInside(int left, int bottom, int right, int top)
        => left <= X && bottom <= Y && X <= right && Y <= top;

}