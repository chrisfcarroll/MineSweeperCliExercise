using System;
using System.Linq;

namespace MineSweeperCli;

public static class ActiveMinesInitializer
{
    public static Position[] RandomFromSizeAndDensityBestEffort(Settings settings)
    {
        settings.SanitiseAndValidateInitialSettingsElseThrow();
        var boardSize = settings.BoardSize;
        var numberOfMinesRequested = boardSize * boardSize * settings.MineDensityPercent / 100;
        var rnd = Random.Shared;
        var activeMinesList = Enumerable
            .Range(1, numberOfMinesRequested)
            .Select(i => new Position(1 + rnd.Next(boardSize), 1 + rnd.Next(boardSize)))
            .OrderBy(x=>x.X).ThenBy(x=>x.Y)
            .Distinct()
            .ToArray();
        return activeMinesList;
    }
}