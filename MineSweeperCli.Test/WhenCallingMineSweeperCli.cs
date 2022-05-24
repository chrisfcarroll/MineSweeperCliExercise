using System.Collections.Generic;
using System.Linq;
using Extensions.Logging.ListOfString;
using TestBase;
using Xunit;

namespace MineSweeperCli.Test
{
    public class WhenCallingMineSweeperCli
    {
        [Fact]
        public void ShouldLog()
        {
            //Act
            unitUnderTest.Do();
            
            //Assert
            log
                .ShouldNotBeEmpty()
                .First().ShouldNotBeEmpty();
        }

        public WhenCallingMineSweeperCli()
        {
            unitUnderTest = new MineSweeperCli(
                new StringListLogger(log = new List<string>()), 
                new Settings());
        }

        readonly MineSweeperCli unitUnderTest;
        readonly List<string> log;
    }
}