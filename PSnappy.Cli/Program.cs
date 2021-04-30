using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
using System.Threading.Tasks;
using PSnappy.Cli.About;
using PSnappy.Cli.TotalM1VisualizationResults;

namespace PSnappy.Cli
{
    static class Program
    {
        private static bool _interactive;

        static async Task Main(string[] args)
        {
            var root = new RootCommand();

            var interactive = new Command("i", "enter interactive mode with the CLI");
            interactive.Handler = CommandHandler.Create(() => _interactive = true);

            root.AddCommand(AboutCommands.Instance);
            root.AddCommand(TotalM1VisualizationResultsCommands.Instance);

            var builder = new CommandLineBuilder(root)
                .UseDefaults()
                .UseMiddleware(async (invocation, next) => await next(invocation))
                .AddCommand(interactive);

            await builder.Build().InvokeAsync(args);

            if (!_interactive)
            {
                return;
            }

            var exit = new Command("exit", "exit interactive mode");
            exit.Handler = CommandHandler.Create(() => _interactive = false);

            Console.WriteLine(">> Entering interactive mode, type 'exit' to exit.");

            builder.AddCommand(exit);

            var interactiveBuilt = builder.Build();

            while (_interactive)
            {
                Console.Write(">> ");
                var argsString = Console.ReadLine();
                await interactiveBuilt.InvokeAsync(argsString);
            }
        }
    }
}
