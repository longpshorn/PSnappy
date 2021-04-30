using Autofac;
using PSnappy;
using PSnappy.Cli.GenericSqlJob;
using PSnappy.Jobs.TotalM1VisualizationResults;

namespace PSnappy.Cli.TotalM1VisualizationResults
{
    public class TotalM1VisualizationResultsJobFactory : IGenericSqlJobFactory<Job>
    {
        private readonly ILifetimeScope _scope;

        public TotalM1VisualizationResultsJobFactory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public Job CreateJob(SqlJobArguments args)
        {
            return _scope.Resolve<Job>(TypedParameter.From(args));
        }
    }
}
