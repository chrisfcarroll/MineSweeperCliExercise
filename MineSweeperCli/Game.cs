using System;
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