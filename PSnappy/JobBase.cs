using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace PSnappy
{
    public class JobBase : IJob
    {
        protected readonly ILifetimeScope _scope;
        protected readonly IRunManager _runManager;
        protected readonly IStatusLogger _logger;

        protected IRunOptions _runOptions;
        protected ILifetimeScope _runScope;
        protected IJobRunner _runner;

        public JobBase(SqlJobArguments args)
        {
            var container = JobRunner.GetDefaultContainer(args);
            _scope = container.Build().BeginLifetimeScope();
            _logger = _scope.Resolve<IStatusLogger>();
            _logger.UpdateProcessId(args.ProcessId);
            _runManager = _scope.Resolve<IRunManager>();
        }

        public IEnumerable<StatusEventArgs> StatusLog
        {
            get
            {
                return _logger?.StatusLog ?? Enumerable.Empty<StatusEventArgs>();
            }
        }

        public virtual void Commit()
        {
            if (Validate())
            {
                _runner.Commit();
            }
        }

        public virtual void Reset()
        {
            _runScope?.Dispose(); //dispose of any previous
            _runOptions = _runManager.GetRunOptions();
            _logger.UpdateProcessId(_runOptions.ProcessId);
        }

        public virtual async Task<bool> RunAsync()
        {
            if (Validate())
            {
                return await _runner.RunAsync();
            }
            else
            {
                return false;
            }
        }

        protected virtual bool Validate()
        {
            if (_runner != null)
            {
                return true; // don't need to do anything as a reset has already occurred and we are populated
            }

            this.Reset();
            return true;
        }
    }
}
