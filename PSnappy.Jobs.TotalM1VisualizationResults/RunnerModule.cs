using Autofac;
using PSnappy;

namespace PSnappy.Jobs.TotalM1VisualizationResults
{
    public class RunnerModule : Module
    {
        private readonly IRunOptions _runOptions;

        public RunnerModule(IRunOptions runOptions)
        {
            _runOptions = runOptions;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterInstance(_runOptions).As<IRunOptions>();
            builder.RegisterType<JobRunner>().AsImplementedInterfaces().SingleInstance();

            builder.RegisterType<DatasetBuilder>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DatasetCalculator>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DatasetCommitter>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DatasetContext>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DatasetReporter>().AsImplementedInterfaces().InstancePerLifetimeScope();
            builder.RegisterType<DatasetWriter>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}
