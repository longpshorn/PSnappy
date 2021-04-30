using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Reflection;

namespace PSnappy.Cli.About
{
    public class AboutCommands
    {
        public static Command Instance = About();

        private static Command About()
        {
            var root = new Command("about", "provide details about where this assembly lives, when it was updated, etc.");

            root.Handler = CommandHandler.Create(() =>
            {
                var assembly = Assembly.GetExecutingAssembly();
                var fileInfo = new FileInfo(assembly.Location);

                Console.WriteLine($"{nameof(assembly.FullName).PadRight(20)}: {assembly.FullName}");
                Console.WriteLine($"{nameof(assembly.Location).PadRight(20)}: {assembly.Location}");
                Console.WriteLine($"{nameof(fileInfo.LastWriteTime).PadRight(20)}: {fileInfo.LastWriteTime}");
            });

            return root;
        }
    }
}
