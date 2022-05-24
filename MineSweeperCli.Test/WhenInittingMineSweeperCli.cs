using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using TestBase;
using Xunit;
using Xunit.Abstractions;
using Assert = Xunit.Assert;

namespace MineSweeperCli.Test
{
    public class WhenInittingMineSweeperCli
    {
        [Theory]
        [InlineData(-1,1,1)]
        [InlineData(1,-1,1)]
        [InlineData(1,1,-1)]
        [InlineData(0,1,1)]
        [InlineData(1,0,1)]
        [InlineData(1,1,0)]
        [InlineData(1,1,2)]
        [InlineData(27,1,1)]
        public void ShouldThrowGivenInvalidBoardSize(int boardSize, int startingLives, int startingColumn)
        {
            //Arrange
            var settings = new Settings
            {
                BoardSize = boardSize, 
                StartingLives = startingLives,
                StartingColumn = startingColumn
            };
            
            //Debug
            outt.WriteLine(settings.ToString());
            //Act
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                new Game(new StringListLogger(log = new List<string>()), settings));
            
        }
        
        [Theory]
        [InlineData(8,2,4)]
        public void ShouldInitAndNotThrowGivenValidSettings(int boardSize, int startingLives, int startingColumn)
        {
            //Arrange
            var settings = new Settings
            {
                BoardSize = boardSize, 
                StartingLives = startingLives,
                StartingColumn = startingColumn
            };

            
            //Act
            var unitUnderTest = new Game(
                new StringListLogger(log = new List<string>()), 
                settings);
            unitUnderTest.GetStatusLine().ShouldNotBeNullOrEmptyOrWhitespace();
            
            //Assert
            log.ShouldNotBeEmpty().First().ShouldNotBeEmpty();
        }

        [Theory]
        [InlineData(8, 10)]
        public void ThereShouldBeMinesPlaced(int boardSize, int mineDensityPercent)
        {
            //Arrange
            var settings = new Settings
            {
                BoardSize = boardSize, 
                MineDensityPercent = mineDensityPercent
            };

            
            //Act
            var gameUnderTest = new Game(
                new StringListLogger(log = new List<string>()), 
                settings);
            
            //debug
            gameUnderTest.ActiveMines.ShouldNotBeNull("Active Mines was null");
            outt.WriteLine($"Boardsize: {boardSize} | MineDensityPercent {mineDensityPercent} ");
            outt.WriteLine(string.Join(",", gameUnderTest.ActiveMines));
            
            //Assert
            gameUnderTest.ActiveMines.Count.ShouldBeGreaterThanOrEqualTo(1);
            gameUnderTest.ActiveMines.Count.ShouldBeLessThanOrEqualTo((boardSize *
                                                                        boardSize *
                                                                        mineDensityPercent /
                                                                        100));
        }
        
        [Fact]
        public void ShouldInitAndNotThrowAndShouldLogGivenDefaultSettings()
        {
            //Arrange
            var unitUnderTest = new Game(
                new StringListLogger(log = new List<string>()), 
                new Settings());
            
            //Act
            unitUnderTest.GetStatusLine();
            
            //Assert
            log.ShouldNotBeEmpty().First().ShouldNotBeEmpty();
        }
        
        public WhenInittingMineSweeperCli(ITestOutputHelper outt)
        {
            this.outt = outt;
        }

        readonly ITestOutputHelper outt;
        List<string> log;
    }
}