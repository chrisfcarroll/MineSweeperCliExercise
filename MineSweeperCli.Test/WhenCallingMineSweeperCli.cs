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
        readonly ITestOutputHelper outt;

        [Fact]
        public void ShouldReturnStatusLine()
        {
            //Act
            var line = unitUnderTest.GetStatusLine();
            
            //Debug
            outt.WriteLine(line);
            //Assert
            line
                .ShouldBe(
                    string.Format(
                        Game.StatusLineTemplate, unitUnderTest.PlayerPosition, unitUnderTest.LivesLeft));
        }
        
        [Fact]
        public void ShouldLog()
        {
            //Act
            unitUnderTest.GetStatusLine();
            
            //Assert
            log.ShouldNotBeEmpty().First().ShouldNotBeEmpty();
        }

        public WhenCallingMineSweeperCli(ITestOutputHelper outt)
        {
            this.outt = outt;
            unitUnderTest = new Game(
                new StringListLogger(log = new List<string>()), 
                new Settings());
        }

        readonly Game unitUnderTest;
        readonly List<string> log;
    }
}