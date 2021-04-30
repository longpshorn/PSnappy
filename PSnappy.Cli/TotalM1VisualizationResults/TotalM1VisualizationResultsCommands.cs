using System.CommandLine;
using PSnappy.Cli.GenericSqlJob;
using PSnappy.Jobs.TotalM1VisualizationResults;

namespace PSnappy.Cli.TotalM1VisualizationResults
{
    public class TotalM1VisualizationResultsCommands
    {
        public static Command Instance = TotalM1VisualizationResults();

        private static Command TotalM1VisualizationResults()
        {
            var command = new Command("totalm1visualizationresults", "Runs the Total M-1 Visualization Results job");
            command.AddCommand(GenericSqlJobCommands.Commit(ResolveService));
            command.AddCommand(GenericSqlJobCommands.Reset(ResolveService));
            command.AddCommand(GenericSqlJobCommands.Run(ResolveService));
            return command;
        }

        private static GenericSqlJobService<Job> ResolveService()
        {
            return Container.ResolveService<GenericSqlJobService<Job>>();
        }
    }
}
