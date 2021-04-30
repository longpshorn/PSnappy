using Autofac;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class Job : JobBase, IJob
    {
        public Job(SqlJobArguments args)
            : base(args)
        {
            _logger.UpdateProcessName("Total M1 Visualization Results");
        }

        public override void Reset()
        {
            base.Reset();
            _runScope = _scope.BeginLifetimeScope(cb => cb.RegisterModule(new RunnerModule(_runOptions)));
            _runner = _runScope.Resolve<IJobRunner>();
        }
    }
}
