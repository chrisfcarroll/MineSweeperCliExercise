using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using TestBase;
using Xunit;
using Xunit.Abstractions;

namespace MineSweeperCli.Test
{
    public class WhenPlayerMoves
    {
        [Theory]
        [InlineData(ConsoleKey.UpArrow, 0, 1)]
        [InlineData(ConsoleKey.LeftArrow, -1, 0)]
        [InlineData(ConsoleKey.RightArrow, 1, 0)]
        [InlineData(ConsoleKey.DownArrow, 0, -1)]
        public void UpLeftRightDownArrowMovesPlayerUpLeftRight(ConsoleKey key, int expectedMoveX, int expectedMoveY)
        {
            //Arrange
            var outputs = new List<string>();
            controller = new GameController(new StringListLogger(log), initialPosition: new Position(4, 4));
            var positionBefore = controller.Game.PlayerPosition;
            
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
            var expectedNewPosition = new Position(positionBefore.X + expectedMoveX , positionBefore.Y + expectedMoveY);
            controller.Game.PlayerPosition.ShouldBe(
                expectedNewPosition
            );
            if (!controller.Game.ActiveMines.Contains(expectedNewPosition))
            {
                controller.GetStatusLine().ShouldNotContain(GameController.Bang);
            }
        }
        
        [Theory]
        [InlineData(ConsoleKey.UpArrow)]
        public void MoveOffTopOfBoardWinsGame(ConsoleKey key)
        {
            //Arrange
            var outputs = new List<string>();
            controller = new GameController(new StringListLogger(log), initialPosition: new Position(5, 8));
            var positionBefore = controller.Game.PlayerPosition;
            
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
            var expectedNewPosition = new Position(positionBefore.X , positionBefore.Y + 1);
            controller.Game.PlayerPosition.ShouldBe(expectedNewPosition);
            controller.Game.IsWon().ShouldBeTrue();
            controller.GetStatusLine().ShouldNotContain(GameController.Bang);
            controller.GetStatusLine().ShouldContain(GameController.YouWon);
            
        }        
        [Fact]
        public void StatusShouldShowPlayerMoveCount()
        {
            //Arrange
            var keysToType = new[] { ConsoleKey.UpArrow, ConsoleKey.DownArrow };
            using var keysEnumerator = ((IEnumerable<ConsoleKey>)keysToType).GetEnumerator();
            var outputs = new List<string>();
            
            
            //Act
            controller.EventLoop(
                () => keysEnumerator.MoveNext() ? keysEnumerator.Current : throw new Exception(),
                outputs.Add);
            
            //Debug
            outt.WriteLine("LogLines");
            log.ForEach(l => outt.WriteLine(l));
            outt.WriteLine("Player Output");
            outputs.ForEach(l => outt.WriteLine(l));
            
            //Assert
            controller.Game.PlayerMoveCount.ShouldBe(keysToType.Length);

        }

        public WhenPlayerMoves(ITestOutputHelper outt)
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