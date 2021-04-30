using System;
using Autofac;

namespace PSnappy.Cli
{
    public static class Container
    {
        public static T ResolveService<T>(
            Action<ContainerBuilder> preRegister = null,
            Action<ContainerBuilder> callback = null
        )
            where T : class
        {
            var cb = new ContainerBuilder();

            preRegister?.Invoke(cb);

            cb.RegisterModule(new CLIModule());

            var c = cb.Build();

            var scope = c.BeginLifetimeScope();

            scope = scope.BeginLifetimeScope(x =>
            {
                callback?.Invoke(x);
            });

            return scope.Resolve<T>();
        }
    }
}
