using System;
using LogAssert;
using Microsoft.Extensions.Logging;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MineSweeperCli.Test")]

namespace MineSweeperCli;

/// <summary>A component which … </summary>
public class Game
{
    internal const string StatusLineTemplate = "Current Position: {0} | Lives Left {1}";

    public string GetStatusLine()
    {
        log.LogTrace(nameof(GetStatusLine));
        return string.Format(StatusLineTemplate, PlayerPosition, LivesLeft);
    }

    public Game(ILogger log, Settings settings)
    {
        this.log = log;
        log.LogDebug("Created with Settings={@Settings}", settings);
        ValidateInitialSettingsElseThrow(log, settings);
        PlayerPosition = new Position(settings.StartingColumn, 1);
        LivesLeft = settings.StartingLives;
        BoardSize = settings.BoardSize;
    }

    static void ValidateInitialSettingsElseThrow(ILogger log, Settings settings)
    {
        log.EnsureElseThrow(settings.BoardSize >= 1,
            () => new ArgumentOutOfRangeException("settings.BoardSize", "must be at least 1"));
        log.EnsureElseThrow(settings.BoardSize <= 26,
            () => new ArgumentOutOfRangeException("settings.BoardSize", "Sorry, maximum board size is currently 26"));
        log.EnsureElseThrow(settings.StartingLives >= 1,
            () => new ArgumentOutOfRangeException("settings.StartingsLives", "must be at least 1"));
        log.EnsureElseThrow(settings.StartingColumn >= 1,
            () => new ArgumentOutOfRangeException("settings.StartingColumn", "must be at least 1"));
        log.EnsureElseThrow(settings.StartingColumn <= settings.BoardSize,
            () => new ArgumentOutOfRangeException("settings.StartingColumn", "must be at least no bigger than boardSize"));
    }

    public int BoardSize { get; }
    public int LivesLeft { get; }
    public Position PlayerPosition { get; }
    
    readonly ILogger log;
}