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
        [Fact]
        public void UpArrowMovesPlayerUp()
        {
            //Arrange
            var outputs = new List<string>();
            var positionBefore = gameUnderTest.PlayerPosition;
            
            //Act
            var hasMoved = false;
            gameUnderTest.EventLoop( 
                    ()=>
                    {
                        if (hasMoved) throw new Exception();
                        hasMoved = true;
                        return ConsoleKey.UpArrow;
                    }, 
                outputs.Add);
            
            //Debug
            outt.WriteLine($"Before {positionBefore} | After {gameUnderTest.PlayerPosition}");
            
            //Assert
            gameUnderTest.PlayerPosition.ShouldBe(
                new Position(positionBefore.X, positionBefore.Y + 1)
            );

        }
        [Fact]
        public void StatusShouldShowPlayerMoveCount()
        {
            //Arrange
            var keysToType = new[] { ConsoleKey.UpArrow, ConsoleKey.DownArrow };
            using var keysEnumerator = ((IEnumerable<ConsoleKey>)keysToType).GetEnumerator();
            var outputs = new List<string>();
            
            
            //Act
            gameUnderTest.EventLoop(
                () => keysEnumerator.MoveNext() ? keysEnumerator.Current : throw new Exception(),
                outputs.Add);
            
            //Debug
            outt.WriteLine("LogLines");
            log.ForEach(l => outt.WriteLine(l));
            outt.WriteLine("Player Output");
            outputs.ForEach(l => outt.WriteLine(l));
            
            //Assert
            gameUnderTest.PlayerMoveCount.ShouldBe(keysToType.Length);

        }

        public WhenPlayerMoves(ITestOutputHelper outt)
        {
            this.outt = outt;
            gameUnderTest = new Game(
                new StringListLogger(log = new List<string>()), 
                settings);
        }

        Game gameUnderTest;
        List<string> log;
        readonly ITestOutputHelper outt;
        Settings settings = new();
    }
}