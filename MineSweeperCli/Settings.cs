using System;
using LogAssert;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MineSweeperCli
{
    public class Settings
    {
        public int BoardSize { get; set; } = 8;
        public int StartingLives { get; set; } = 3;
        public int StartingColumn { get; set; } = 4;
        public int MineDensityPercent { get; set; } = 9;

        public void SanitiseAndValidateInitialSettingsElseThrow(ILogger? log=null)
        {
            log = log ?? NullLogger.Instance;
            log.EnsureElseThrow(BoardSize >= 1,
                () => new ArgumentOutOfRangeException("BoardSize", "must be at least 1"));
            log.EnsureElseThrow(BoardSize <= 26,
                () => new ArgumentOutOfRangeException("BoardSize", "Sorry, maximum board size is currently 26"));
            log.EnsureElseThrow(StartingLives >= 1,
                () => new ArgumentOutOfRangeException("StartingsLives", "must be at least 1"));
            log.EnsureElseThrow(StartingColumn >= 1,
                () => new ArgumentOutOfRangeException("StartingColumn", "must be at least 1"));
            log.EnsureElseThrow(StartingColumn <= BoardSize,
                () => new ArgumentOutOfRangeException("StartingColumn", "must be at least no bigger than boardSize"));
            MineDensityPercent = MineDensityPercent < 0 ? 0 : MineDensityPercent;
            MineDensityPercent = MineDensityPercent >= 100 ? 100 : MineDensityPercent;
        }
    }
}
