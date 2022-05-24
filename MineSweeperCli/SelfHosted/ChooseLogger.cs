using Microsoft.Extensions.Logging;

namespace MineSweeperCli.SelfHosted
{
    static class ChooseLogger
    {
        public static ILoggerFactory Choose()
        {
            return UseSerilog.GetFactory();
        }
    }
}