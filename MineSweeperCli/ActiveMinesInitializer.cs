using System;
using System.Collections.Generic;
using System.Linq;

namespace MineSweeperCli;

public static class ActiveMinesInitializer
{
    public static List<Position> RandomFromSizeAndDensityBestEffort(Settings settings, Position avoidInitialPosition)
    {
        settings.SanitiseAndValidateInitialSettingsElseThrow();
        var boardSize = settings.BoardSize;
        var numberOfMinesRequested = boardSize * boardSize * settings.MineDensityPercent / 100;
        var rnd = Random.Shared;
        var activeMinesList = Enumerable
            .Range(1, (int)(numberOfMinesRequested * 1.2))
            .Select(i => new Position(1 + rnd.Next(boardSize), 1 + rnd.Next(boardSize)))
            .OrderBy(x=>x.X).ThenBy(x=>x.Y)
            .Distinct()
            .Where(x=>x!= avoidInitialPosition)
            .Take(numberOfMinesRequested)
            .ToList();
        return activeMinesList;
    }
}