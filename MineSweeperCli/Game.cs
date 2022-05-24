using System;
using System.Threading;
using LogAssert;
using Microsoft.Extensions.Logging;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MineSweeperCli.Test")]

namespace MineSweeperCli;

/// <summary>A component which … </summary>
public class Game
{
    internal const string StatusLineTemplate = "Current Position: {0} | Lives Left {1} | Moves Made {2}";

    public string GetStatusLine()
    {
        log.LogTrace(nameof(GetStatusLine));
        return string.Format(StatusLineTemplate, PlayerPosition, LivesLeft, PlayerMoveCount);
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

    /// <summary>Board Size set by constructor <seealso cref="Settings"/></summary>
    public int BoardSize { get; }
    
    /// <summary>Is initally set by constructor <seealso cref="Settings"/> and is updated as you play
    /// </summary>
    public int LivesLeft { get; private set; }
    
    /// <summary>Is initally set by constructor <seealso cref="Settings"/> and is updated as you play</summary>
    public Position PlayerPosition { get; private set; }
    
    /// <summary>Is updated as you play</summary>
    public int PlayerMoveCount { get; private set; }

    readonly ILogger log;

    /// <summary>Game event loop. Connect e.g. a Console Input & and Output to this to play the game 
    /// </summary>
    /// <param name="input">This function will be repeatedly called until it throws or the game ends</param>
    /// <param name="output">Send Player Output text after each move to this output</param>
    /// <param name="cancellationToken">Optional cancellationToken</param>
    /// <returns>the ‘final’ after all input is processed. Note that if input ends before game end then
    /// this returns the current status and the game is still live and can be continued</returns>
    public string EventLoop(Func<ConsoleKey> input,
                            Action<string> output,
                            CancellationToken cancellationToken=default)
    {
        Exception? inputException = null;
        while (!cancellationToken.IsCancellationRequested && inputException is null)
        {
            try
            {
                var move = input();
                PlayerMoveCount += 1;
                var nextOutputLine = string.Join(" | ", $"Moved:{move}", GetStatusLine()); 
                output( nextOutputLine );
                log.LogInformation(nextOutputLine);
            } 
            catch (Exception e)
            {
                inputException = e;
                log.LogInformation("Finished when input throw exception {FinishedWithException}", inputException);
            }
        }
        return GetStatusLine();
    }
}