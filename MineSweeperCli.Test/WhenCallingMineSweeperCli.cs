using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using TestBase;
using Xunit;
using Xunit.Abstractions;

namespace MineSweeperCli.Test
{
    public class WhenCallingMineSweeperCli
    {
        [Theory]
        [InlineData(4)]
        [InlineData(1)]
        public void StatusShouldShowPositionInChessboardFormat(int startingColumn)
        {
            //Arrange
            unitUnderTest = new Game(
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
            unitUnderTest = new Game(
                new StringListLogger(log = new List<string>()), 
                new Settings{StartingLives = startLives});
            
            //Act
            var line = unitUnderTest.GetStatusLine();
            
            //Debug
            outt.WriteLine(line);
            //Assert
            line
                .ShouldBe(
                    string.Format(
                        Game.StatusLineTemplate, unitUnderTest.PlayerPosition, startLives));
        }

        public WhenCallingMineSweeperCli(ITestOutputHelper outt)
        {
            this.outt = outt;
            unitUnderTest = new Game(
                new StringListLogger(log = new List<string>()), 
                settings);
        }

        Game unitUnderTest;
        List<string> log;
        readonly ITestOutputHelper outt;
        Settings settings = new();
    }
}