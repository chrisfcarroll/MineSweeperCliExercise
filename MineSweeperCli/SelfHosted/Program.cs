using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MineSweeperCli.SelfHosted
{
    /// <summary>
    /// A commandline wrapper for <see cref="GameController"/> which uses <see cref="Startup"/> to
    /// initialize Configuration, Logging, and Settings. 
    /// </summary>
    public static class Program
    {
        public static void Main(params string[] args)
        {
            ValidateExampleParametersElseShowHelpTextAndExit(args);
            
            Startup.Configure();
            
            var controller= new GameController(
                    args.Length>0 
                        ? Startup.LoggerFactory.CreateLogger<GameController>()
                        : NullLogger.Instance, 
                    Startup.Settings
                );
            
            Console.WriteLine(controller.GetStatusLine());

            var outcome= controller.EventLoop(() => Console.ReadKey().Key, Console.WriteLine);

            switch (outcome.Progress)
            {
                case GameProgress.Won: 
                    Console.WriteLine("Well done! Final Score " + outcome.Score);
                    break;
                case GameProgress.Lost: 
                    Console.WriteLine("You win some, you lose some!");
                    break;
                default: 
                    Console.WriteLine("Don't Stop Now");
                    break;
            }
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