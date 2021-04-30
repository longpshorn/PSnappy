using Autofac;
using PSnappy.Cli.GenericSqlJob;
using PSnappy.Cli.TotalM1VisualizationResults;

namespace PSnappy.Cli
{
    public class CLIModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<CLILogger>().AsSelf();
            builder.RegisterType<Validator>().AsImplementedInterfaces();

            builder.RegisterType<Jobs.TotalM1VisualizationResults.Job>().AsSelf();

            builder.RegisterType<GenericSqlJobService<Jobs.TotalM1VisualizationResults.Job>>().AsSelf();

            builder.RegisterType<TotalM1VisualizationResultsJobFactory>().AsImplementedInterfaces();
        }
    }
}
