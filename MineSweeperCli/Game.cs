using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using LogAssert;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("MineSweeperCli.Test")]

namespace MineSweeperCli;

/// <summary>A component which … </summary>
public class Game
{
    internal const string Delim = " | ";
    internal const string StatusLineTemplate = "Current Position: {0} | Lives Left {1} | Moves Made {2}";
    internal const string Bang = "BANG!";
    internal const string YouWon = "Congratulations, You Won!";

    public string GetStatusLine()
    {
        log.LogTrace(nameof(GetStatusLine));
        var statusLine = string.Format(StatusLineTemplate, PlayerPosition, LivesLeft, PlayerMoveCount);
        if (IsOnMine()) { statusLine = string.Join(Delim, statusLine, Bang);}
        if (IsWon()) { statusLine = string.Join(Delim, statusLine, YouWon);}
        return statusLine;
    }


    public Game(ILogger? log=null, Settings? settings=null, Position? initialPosition=null)
    {
        this.log = log?? NullLogger.Instance;
        settings ??= new Settings();
        log.LogDebug("Created with Settings={@Settings}", settings);
        settings.SanitiseAndValidateInitialSettingsElseThrow(log);
        if (initialPosition is Position initialPositionV)
        {
            log.EnsureElseThrow(initialPositionV.IsInside(1, 1, settings.BoardSize, settings.BoardSize),
                ()=> new ArgumentOutOfRangeException("initialPosition", "Initial position must be on the board"),initialPosition);
            PlayerPosition = initialPositionV ;
        }
        else
        {
            PlayerPosition = new Position(settings.StartingColumn, 1);
        }
        LivesLeft = settings.StartingLives;
        BoardSize = settings.BoardSize;
        ActiveMines = ActiveMinesInitializer.RandomFromSizeAndDensityBestEffort(settings,PlayerPosition);
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

    /// <summary>Is created during construction, with mine density taken from <see cref="Settings.MineDensityPercent"/>
    /// </summary>
    public List<Position> ActiveMines { get; internal set; }

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
        while (!cancellationToken.IsCancellationRequested && inputException is null && !GameOver)
        {
            try
            {
                var move = input();
                UpdatePositionFrom(move);
                PlayerMoveCount += 1;
                var nextOutputLine = string.Join(" | ", $"Moved:{move}", GetStatusLine()); 
                output( nextOutputLine );
                log.LogInformation(nextOutputLine);
                LoseLifeIfDead();
            } 
            catch (Exception e)
            {
                inputException = e;
                log.LogInformation("Finished when input throw exception {FinishedWithException}", inputException);
            }
        }
        return GetStatusLine();
    }

    void LoseLifeIfDead()
    {
        if (IsOnMine())
        {
            log.LogInformation("Hit Mine at {PlayerPosition}", PlayerPosition);
            LivesLeft -= 1;
        }
    }

    public bool IsOnMine() => ActiveMines.Contains(PlayerPosition);
    public bool IsWon() => LivesLeft > 0 && PlayerPosition.Y > BoardSize;

    public bool GameOver => LivesLeft == 0 || PlayerPosition.Y > BoardSize;

    void UpdatePositionFrom(ConsoleKey move)
    {
        switch (move)
        {
            case ConsoleKey.UpArrow : MoveUpIfPossible();
                break;
            case ConsoleKey.LeftArrow : MoveLeftIfPossible();
                break;
            case ConsoleKey.RightArrow : MoveRightIfPossible();
                break;
            case ConsoleKey.DownArrow : MoveDownIfPossible();
                break;
        }
    }

    void MoveUpIfPossible()
    {
        if (PlayerPosition.Y <= BoardSize)
        {
            PlayerPosition = PlayerPosition.Add(0, 1);
        }   
    }
    void MoveLeftIfPossible()
    {
        if (PlayerPosition.X > 1)
        {
            PlayerPosition = PlayerPosition.Add(-1, 0);
        }   
    }
    void MoveRightIfPossible()
    {
        if (PlayerPosition.X < BoardSize)
        {
            PlayerPosition = PlayerPosition.Add(1, 0);
        }   
    }
    void MoveDownIfPossible()
    {
        if (PlayerPosition.Y > 1)
        {
            PlayerPosition = PlayerPosition.Add(0, -1);
        }   
    }
}