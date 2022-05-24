using System;
using Microsoft.Extensions.Logging;

namespace MineSweeperCli;

/// <summary>A component which … </summary>
public class MineSweeperCli
{
    public void Do()
    {
        log.LogDebug("Called on {@OS}", Environment.OSVersion);
    }

    public MineSweeperCli(ILogger log, Settings settings)
    {
        this.log = log;
        this.settings = settings;
        log.LogDebug("Created with Settings={@Settings}", settings);
        PlayerPosition = new Position(settings.StartingColumn, 1);
        LivesLeft = settings.StartingLives;
        BoardSize = settings.BoardSize;
    }

    public int BoardSize { get; }
    public int LivesLeft { get; }
    public Position PlayerPosition { get; }
    
    readonly ILogger log;
    readonly Settings settings;
}

public record struct Position(int X, int Y);