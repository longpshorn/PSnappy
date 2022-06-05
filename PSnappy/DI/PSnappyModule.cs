using Autofac;
using PSnappy.Common;

namespace PSnappy
{
    public class PSnappyModule : Module
    {
        private readonly bool _simpleLoad = false;
        private readonly SqlOptions _sqlOptions;

        public PSnappyModule()
        {
            _simpleLoad = true;
        }

        public PSnappyModule(SqlOptions sqlOptions)
        {
            _sqlOptions = sqlOptions;
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            if (_simpleLoad)
            {
                builder.RegisterType<SqlHelper>().AsImplementedInterfaces().SingleInstance();
            }
            else
            {
                builder.RegisterInstance(_sqlOptions).As<ISqlOptions>();
                builder.RegisterType<StatusLogger>().As<IStatusLogger>().AsSelf().SingleInstance();
                builder.RegisterType<TimingHelper>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<RunManager>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<DatasetBuildHelper>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<DatasetWriteHelper>().AsImplementedInterfaces().SingleInstance();
                builder.RegisterType<SqlHelper>().AsImplementedInterfaces().SingleInstance();
            }
        }
    }
}
