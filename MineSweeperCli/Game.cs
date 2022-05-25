using System.Collections.Generic;

namespace MineSweeperCli;

/// <summary>A Minesweeper Game
/// How to play: Move with MoveXXX actions and read status from IsXXX status properties
/// </summary>
public class Game
{
    public void MoveUpIfPossible()
    {
        if (GameOver || PlayerPosition.Y > BoardSize) return;
        PlayerPosition = PlayerPosition.Add(0, 1);
        UpdateMoveCountCheckIfDead();
    }

    public void MoveLeftIfPossible()
    {
        if (GameOver || PlayerPosition.X <= 1) return;
        PlayerPosition = PlayerPosition.Add(-1, 0);
        UpdateMoveCountCheckIfDead();
    }
    public void MoveRightIfPossible()
    {
        if (GameOver || PlayerPosition.X >= BoardSize) return;
        PlayerPosition = PlayerPosition.Add(1, 0);
        UpdateMoveCountCheckIfDead();
    }
    public void MoveDownIfPossible()
    {
        if (GameOver || PlayerPosition.Y <= 1) return;
        PlayerPosition = PlayerPosition.Add(0, -1);
        UpdateMoveCountCheckIfDead();
    }
    
    /// <summary>Ouch! Lose a life.</summary>
    public bool IsOnMine => ActiveMines.Contains(PlayerPosition);
    
    /// <summary>Win by reaching the top of the board</summary>
    public bool IsWon => LivesLeft > 0 && PlayerPosition.Y > BoardSize;
    
    /// <summary> True if all lives are lost or if you made it to the top of the board </summary>
    public bool GameOver => LivesLeft == 0 || PlayerPosition.Y > BoardSize;

    public GameProgress Progress
        => IsWon 
            ? GameProgress.Won 
            : LivesLeft == 0 
                ? GameProgress.Lost 
                : GameProgress.InProgress;
    
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

    void UpdateMoveCountCheckIfDead()
    {
        if (IsOnMine) { LivesLeft -= 1; }
        PlayerMoveCount++;
    }
    
    public Game(Settings settings, List<Position> activeMines, Position initialPosition)
    {
        BoardSize = settings.BoardSize;
        LivesLeft = settings.StartingLives;
        PlayerPosition = initialPosition;
        ActiveMines = activeMines;
    }
}

public enum GameProgress { InProgress, Won, Lost}

public readonly record struct GameResult(GameProgress Progress, int Score);