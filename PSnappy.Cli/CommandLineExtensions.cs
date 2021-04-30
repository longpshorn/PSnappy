using System.CommandLine;

namespace PSnappy.Cli
{
    public static class CommandLineExtensions
    {
        public static Command AddSettingsOptions(this Command cmd) => cmd.AddSettingsOptions("");

        public static Command AddSettingsOptions<T>(this Command cmd, T defaultValue = default(T))
        {
            return cmd
                .AddOption<T>(
                    new[] { "-s", "--server" },
                    description: "the central server",
                    defaultValue: defaultValue
                )
                .AddOption<T>(
                    new[] { "-d", "--database" },
                    description: "the central database",
                    defaultValue: defaultValue
                )
                .AddOption<T>(
                    new[] { "-en", "--env-name" },
                    description: "the environment name",
                    defaultValue: defaultValue
                )
            ;
        }

        public static Command AddQueueOptions(this Command cmd)
        {
            return cmd
                .AddOption<string>(
                    new[] { "-s", "--server" },
                    description: "the family database server"
                )

                .AddOption<string>(
                    new[] { "-d", "--database" },
                    description: "the family database"
                )

                .AddOption<bool>(
                    new[] { "--wait" },
                    description: "wait for the queued message to get completed"
                )

                .AddOption<bool>(
                    new[] { "--force" },
                    description: "allow non-local usage of queueing."
                )
            ;
        }

        public static Command AddOption<T>(this Command command, string alias, string description, T defaultValue = default(T))
        {
            return command.AddOption(new[] { alias }, description, defaultValue);
        }

        public static Command AddOption<T>(this Command command, string[] aliases, string description, T defaultValue = default(T))
        {
            command.AddOption(new Option(aliases, description)
            {
                Argument = new Argument<T>(() => defaultValue),
            });

            return command;
        }
    }
}
