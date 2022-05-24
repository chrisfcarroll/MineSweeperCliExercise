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
        public void UpLeftRightArrowMovesPlayerUpLeftRight(ConsoleKey key, int expectedMoveX, int expectedMoveY)
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
                        return key;
                    }, 
                outputs.Add);
            
            //Debug
            outt.WriteLine($"Before {positionBefore} | After {gameUnderTest.PlayerPosition}");
            
            //Assert
            gameUnderTest.PlayerPosition.ShouldBe(
                new Position(positionBefore.X + expectedMoveX , positionBefore.Y + expectedMoveY)
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