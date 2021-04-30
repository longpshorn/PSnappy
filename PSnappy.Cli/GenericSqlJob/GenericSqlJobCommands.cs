using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace PSnappy.Cli.GenericSqlJob
{
    public static class GenericSqlJobCommands
    {
        public delegate IGenericSqlJobService ResolveServiceFunc();

        public static Command Commit(ResolveServiceFunc ResolveService)
        {
            var command = new Command("commit", "perform a commit operation to finalize the run.")
                .AddGenericSqlJobCommitOptions();

            command.Handler = CommandHandler.Create((GenericSqlJobCommitRequest request) =>
            {
                ResolveService().Commit(request);
            });

            return command;
        }

        public static Command Reset(ResolveServiceFunc ResolveService)
        {
            var command = new Command("reset", "perform a reset operation to create a new run")
                .AddGenericSqlJobResetOptions();

            command.Handler = CommandHandler.Create((GenericSqlJobResetRequest request) =>
            {
                ResolveService().Reset(request);
            });

            return command;
        }

        public static Command Run(ResolveServiceFunc ResolveService)
        {
            var command = new Command("run", "run the calc engine for the given latest run.")
                .AddGenericSqlJobRunOptions();

            command.Handler = CommandHandler.Create(async (GenericSqlJobRunRequest request) =>
            {
                await ResolveService().RunAsync(request);
            });

            return command;
        }
    }

    public static class GenericSqlJobOptionsExtensions
    {
        private static Command AddGenericSqlJobOptions(this Command cmd)
        {
            return cmd
                .AddOption<string>(
                    new[] { "-s", "--server" },
                    description: "the database server"
                )

                .AddOption<string>(
                    new[] { "-d", "--database" },
                    description: "the database"
                )

                .AddOption<string>(
                    new[] { "-n", "--name" },
                    description: "the name of the run"
                )

                .AddOption<string>(
                    new[] { "-u", "--user", "--user-name" },
                    description: "the user name of the run"
                )
            ;
        }

        public static Command AddGenericSqlJobCommitOptions(this Command cmd)
        {
            return cmd
                .AddGenericSqlJobOptions()
            ;
        }

        public static Command AddGenericSqlJobResetOptions(this Command cmd)
        {
            return cmd
                .AddGenericSqlJobOptions()

                .AddOption<Guid>(
                    new[] { "--pid", "-p", "--process-id" },
                    description: "the process ID to use (if unprovided, a new one is created.)"
                )
            ;
        }

        public static Command AddGenericSqlJobRunOptions(this Command cmd)
        {
            return cmd
                .AddGenericSqlJobCommitOptions()
            ;
        }
    }
}
