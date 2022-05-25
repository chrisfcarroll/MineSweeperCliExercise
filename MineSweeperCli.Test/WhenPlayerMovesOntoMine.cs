using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using TestBase;
using Xunit;
using Xunit.Abstractions;

namespace MineSweeperCli.Test
{
    public class WhenPlayerMovesOntoMine
    {
        [Theory]
        [InlineData(ConsoleKey.UpArrow, 0, 1)]
        [InlineData(ConsoleKey.LeftArrow, -1, 0)]
        [InlineData(ConsoleKey.RightArrow, 1, 0)]
        [InlineData(ConsoleKey.DownArrow, 0, -1)]
        public void MineGoesBang(ConsoleKey key, int expectedMoveX, int expectedMoveY)
        {
            //Arrange
            var outputs = new List<string>();
            controller = new GameController(new StringListLogger(log), initialPosition: new Position(4, 4));
            var positionBefore = controller.Game.PlayerPosition;
            var nextPosition = new Position(positionBefore.X + expectedMoveX , positionBefore.Y + expectedMoveY);
            controller.Game.ActiveMines.Add(nextPosition);
            
            //Act
            var hasMoved = false;
            controller.EventLoop( 
                ()=>
                {
                    if (hasMoved) throw new Exception();
                    hasMoved = true;
                    return key;
                }, 
                outputs.Add);
            
            //Debug
            outt.WriteLine($"Before {positionBefore} | After {controller.Game.PlayerPosition}");
            
            //Assert
            controller.Game.PlayerPosition.ShouldBe(nextPosition);
            controller.GetStatusLine().ShouldContain(GameController.Bang);
        }
        
        [Theory]
        [InlineData(ConsoleKey.UpArrow, 0, 1)]
        [InlineData(ConsoleKey.LeftArrow, -1, 0)]
        [InlineData(ConsoleKey.RightArrow, 1, 0)]
        [InlineData(ConsoleKey.DownArrow, 0, -1)]
        public void LastLifeEndsGame(ConsoleKey key, int expectedMoveX, int expectedMoveY)
        {
            //Arrange
            var outputs = new List<string>();
            controller = new GameController(
                new StringListLogger(log),
                new Settings{StartingLives = 1},
                initialPosition: new Position(4, 4));
            var positionBefore = controller.Game.PlayerPosition;
            var nextPosition = new Position(positionBefore.X + expectedMoveX , positionBefore.Y + expectedMoveY);
            controller.Game.ActiveMines.Add(nextPosition);
            
            //Act
            var hasMoved = false;
            controller.EventLoop( 
                ()=>
                {
                    if (hasMoved) throw new Exception();
                    hasMoved = true;
                    return key;
                }, 
                outputs.Add);
            
            //Debug
            outt.WriteLine($"Before {positionBefore} | After {controller.Game.PlayerPosition}");
            
            //Assert
            controller.Game.PlayerPosition.ShouldBe(nextPosition);
            controller.GetStatusLine().ShouldContain(GameController.Bang);
            controller.Game.GameOver.ShouldBeTrue();
            controller.Game.IsWon().ShouldBeFalse();
        }


        public WhenPlayerMovesOntoMine(ITestOutputHelper outt)
        {
            this.outt = outt;
            controller = new GameController(
                new StringListLogger(log = new List<string>()), 
                settings);
        }

        GameController controller;
        List<string> log;
        readonly ITestOutputHelper outt;
        Settings settings = new();
    }
}