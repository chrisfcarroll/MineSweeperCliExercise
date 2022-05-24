using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MineSweeperCli.SelfHosted
{
    /// <summary>
    /// A commandline wrapper for <see cref="Game"/> which uses <see cref="Startup"/> to
    /// initialize Configuration, Logging, and Settings. 
    /// </summary>
    public static class Program
    {
        public static void Main(params string[] args)
        {
            ValidateExampleParametersElseShowHelpTextAndExit(args);
            
            Startup.Configure();
            
            var game= new Game(
                    args.Length>0 
                        ? Startup.LoggerFactory.CreateLogger<Game>()
                        : NullLogger.Instance, 
                    Startup.Settings
                );
            
            Console.WriteLine(game.GetStatusLine());

            game.EventLoop(() => Console.ReadKey().Key, Console.WriteLine);
        }

        static void ValidateExampleParametersElseShowHelpTextAndExit(string[] args)
        {
            ShowHelpTextAndExitImmediatelyIf(shouldShowHelpThenExit: args.Length > 1);
        }

        static readonly string[] HelpOptions = {"?", "h","help"};

        const string ConsoleHelpText = @"
MineSweeperCli Chris F Carroll 24 May 2022

    Use the Arrow keys to move
    Try to stay alive
";

        /// <summary>
        ///If <paramref name="shouldShowHelpThenExit"/> is not true, then show <see cref="ConsoleHelpText"/> and call
        /// <see cref="Environment.Exit"/> with <c>ExitCode</c>==<see cref="ReturnExitCodeIfParametersInvalid"/>  
        /// </summary>
        /// <param name="shouldShowHelpThenExit"></param>
        static void ShowHelpTextAndExitImmediatelyIf(bool shouldShowHelpThenExit)
        {
            Console.WriteLine(ConsoleHelpText);
            if (!shouldShowHelpThenExit) return;
            Environment.Exit(ReturnExitCodeIfParametersInvalid);
        }

        const int ReturnExitCodeIfParametersInvalid = 1;
    }
}