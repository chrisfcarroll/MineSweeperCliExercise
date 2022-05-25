using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using TestBase;
using Xunit;
using Xunit.Abstractions;

namespace MineSweeperCli.Test
{
    public class WhenReportingMineSweeperCliStatus
    {
        [Theory]
        [InlineData(4)]
        [InlineData(1)]
        public void StatusShouldShowPositionInChessboardFormat(int startingColumn)
        {
            //Arrange
            unitUnderTest = new GameController(
                new StringListLogger(log = new List<string>()), 
                new Settings{BoardSize = 8, StartingColumn = startingColumn});

            var expected = "A"+startingColumn.ToString();
            
            //Act
            var line = unitUnderTest.GetStatusLine();
            
            //Debug
            outt.WriteLine(line);
            //Assert
            line.ShouldContain(expected);
        }
        
        [Theory]
        [InlineData(4)]
        [InlineData(1)]
        public void StatusShouldShowLivesLeft(int startLives)
        {
            //Arrange
            unitUnderTest = new GameController(
                new StringListLogger(log = new List<string>()), 
                new Settings{StartingLives = startLives});
            
            //Act
            var line = unitUnderTest.GetStatusLine();
            
            //Debug
            outt.WriteLine(line);
            //Assert
            line
                .ShouldStartWith(
                    string.Format(
                        GameController.StatusLineTemplate, unitUnderTest.Game.PlayerPosition, startLives, unitUnderTest.Game.PlayerMoveCount));
        }

        public WhenReportingMineSweeperCliStatus(ITestOutputHelper outt)
        {
            this.outt = outt;
            unitUnderTest = new GameController(
                new StringListLogger(log = new List<string>()), 
                settings);
        }

        GameController unitUnderTest;
        List<string> log;
        readonly ITestOutputHelper outt;
        Settings settings = new();
    }
}