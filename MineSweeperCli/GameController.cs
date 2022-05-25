using System;
using System.Linq;
using System.Threading;
using LogAssert;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MineSweeperCli.Test")]

namespace MineSweeperCli;

/// <summary>A component which Handles I/O to a MineSweeper Game </summary>
public class GameController
{
    /// <summary>Game event loop. Connect e.g. a Console Input & and Output to this to play the game 
    /// </summary>
    /// <param name="input">This function will be repeatedly called until it throws or the game ends</param>
    /// <param name="output">Send Player Output text after each move to this output</param>
    /// <param name="cancellationToken">Optional cancellationToken</param>
    /// <returns>the ‘final’ after all input is processed. Note that if input ends before game end then
    /// this returns the current status and the game is still live and can be continued</returns>
    public GameResult EventLoop(Func<ConsoleKey> input,
                                Action<string> output,
                                CancellationToken cancellationToken=default)
    {
        Exception? inputException = null;
        while (!cancellationToken.IsCancellationRequested && inputException is null && !Game.GameOver)
        {
            try
            {
                var move = input();
                UpdatePositionFrom(move);
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
        return new GameResult(Game.Progress, Game.PlayerMoveCount);
    }


    void UpdatePositionFrom(ConsoleKey move)
    {
        switch (move)
        {
            case ConsoleKey.UpArrow : Game.MoveUpIfPossible();
                break;
            case ConsoleKey.LeftArrow : Game.MoveLeftIfPossible();
                break;
            case ConsoleKey.RightArrow : Game.MoveRightIfPossible();
                break;
            case ConsoleKey.DownArrow : Game.MoveDownIfPossible();
                break;
        }
    }

    public string GetStatusLine()
    {
        log.LogTrace(nameof(GetStatusLine));
        var statusLine = string.Format(StatusLineTemplate, Game.PlayerPosition, Game.LivesLeft, Game.PlayerMoveCount);
        if (Game.IsOnMine()) { statusLine = string.Join(Delim, statusLine, Bang);}
        if (Game.IsWon()) { statusLine = string.Join(Delim, statusLine, YouWon);}
        return statusLine;
    }


    public GameController(ILogger? log=null, Settings? settings=null, Position? initialPosition=null)
    {
        this.log = log?? NullLogger.Instance;
        settings ??= new Settings();
        log.LogDebug("Created with Settings={@Settings}", settings);
        settings.SanitiseAndValidateInitialSettingsElseThrow(log);
        if (initialPosition is Position initialPositionV)
        {
            log.EnsureElseThrow(initialPositionV.IsInside(1, 1, settings.BoardSize, settings.BoardSize),
                ()=> new ArgumentOutOfRangeException("initialPosition", "Initial position must be on the board"),initialPosition);
            initialPosition = initialPositionV ;
        }
        else
        {
            initialPosition = new Position(settings.StartingColumn, 1);
        }
        var activeMines = ActiveMinesInitializer.RandomFromSizeAndDensityBestEffort(settings,initialPosition.Value);
        Game = new Game(settings, activeMines, initialPosition.Value);
    }

    internal Game Game;
    readonly ILogger log;
    internal const string Delim = " | ";
    internal const string StatusLineTemplate = "Current Position: {0} | Lives Left {1} | Moves Made {2}";
    internal const string Bang = "BANG!";
    internal const string YouWon = "Congratulations, You Won!";
}