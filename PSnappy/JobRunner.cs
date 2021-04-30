using System;
using System.Diagnostics;
using System.Runtime;
using System.Threading;
using System.Threading.Tasks;
using Autofac;

namespace PSnappy
{
    public interface IJobRunner
    {
        void Commit();
        void Reset();
        Task<bool> RunAsync();
    }

    public class JobRunner : IJobRunner
    {
        protected readonly IRunOptions _options;
        protected readonly ILifetimeScope _scope;
        protected readonly IStatusLogger _logger;
        protected readonly ITimingHelper _timingHelper;

        private readonly string _server;
        private readonly string _database;

        public JobRunner(
            IRunOptions options,
            ILifetimeScope scope,
            IStatusLogger logger,
            ITimingHelper timingHelper
        )
        {
            _server = options.Server;
            _database = options.Database;

            _options = options;
            _scope = scope;
            _logger = logger;
            _timingHelper = timingHelper;

            _logger.LogStatus("Execution started", StatusType.Start);

            if (!GCSettings.IsServerGC)
            {
                _logger.LogStatus("WARNING: This system is not running in server mode.", StatusType.Warning);
            }

            if (!Environment.Is64BitProcess)
            {
                _logger.LogStatus("WARNING: This is running as a 32 bit process", StatusType.Warning);
            }

            GCSettings.LatencyMode = GCLatencyMode.Interactive;
            var process = Process.GetCurrentProcess();
            process.PriorityBoostEnabled = true;
            process.PriorityClass = ProcessPriorityClass.High;

            _logger.LogStatus("Setting min threads to 500, 100");
            ThreadPool.SetMinThreads(500, 100); // this helps perf drastically for larger clients
        }

        public static ContainerBuilder GetDefaultContainer(SqlJobArguments args)
        {
            var sqlOptions = new SqlOptions(args.Server, args.Database, args.UserName);
            var cb = new ContainerBuilder();
            cb.RegisterModule(new PSnappyModule(sqlOptions));
            return cb;
        }

        public virtual void Commit()
        {
            var elapsed = TimeSpan.Zero;

            var context = _scope.Resolve<IDatasetContext>();
            var committer = _scope.Resolve<IDatasetCommitter>();

            try
            {
                elapsed = _timingHelper.TimeThis(() => committer.Commit(context));
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
#if DEBUG
                throw;
#endif
            }
            finally
            {
                _logger.LogStatus($"Committed in {elapsed:m\\:ss}", elapsed, StatusType.Complete);
            }
        }

        public virtual void Reset()
        {
            var w = _scope.Resolve<IDatasetWriter>();
            var r = _scope.Resolve<IDatasetReporter>();
            try
            {
                w.Reset(_server, _database);
                r.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogStatus("An error occurred while resetting output tables", StatusType.Error);
                _logger.LogStatus(ex.ToString(), StatusType.Error);
            }

            _logger.LogStatus("Output tables reset");
        }

        public virtual async Task<bool> RunAsync()
        {
            var elapsed = TimeSpan.Zero;
            bool result = false;

            var context = _scope.Resolve<IDatasetContext>();

            try
            {
                elapsed = await _timingHelper.TimeThisAsync(async () =>
                {
                    if (await BuildAsync(context))
                    {
                        Calculate(context);
                        await WriteAsync(context);

                        result = true;
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
#if DEBUG
                throw;
#endif
            }
            finally
            {
                _logger.LogStatus($"Finished in {elapsed:m\\:ss}", elapsed, StatusType.Complete);
            }

            return result;
        }

        private async Task<bool> BuildAsync(IDatasetContext context)
        {
            if (context == null)
            {
                return false;
            }

            var builder = _scope.Resolve<IDatasetBuilder>();

            try
            {
                var elapsed = await _timingHelper.TimeThisAsync(async () => await builder.BuildAsync(context, _server, _database));
                _logger.LogStatus("Dataset context constructed", elapsed);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogStatus(string.Format("Error constructing dataset context from {0}\\{1}", _server, _database), StatusType.Error);
                _logger.LogStatus(ex.ToString());
                return false;
            }
        }

        private void Calculate(IDatasetContext context)
        {
            if (context != null)
            {
                var iserror = false;
                var sw = Stopwatch.StartNew();
                try
                {
                    var c = _scope.Resolve<IDatasetCalculator>();
                    c.Calculate(context);
                }
                catch (Exception ex)
                {
                    _logger.LogStatus("An error occured during calculation", StatusType.Error);
                    _logger.LogStatus(ex.ToString());
                    iserror = true;
                }

                sw.Stop();

                if (iserror)
                {
                    _logger.LogStatus("Dataset calculated with errors", sw.Elapsed, StatusType.Error);
                }
                else
                {
                    _logger.LogStatus("Dataset calculated", sw.Elapsed);
                }
            }
        }

        private async Task WriteAsync(IDatasetContext context)
        {
            if (context != null)
            {
                var iserror = false;
                var sw = Stopwatch.StartNew();
                try
                {
                    var w = _scope.Resolve<IDatasetWriter>();
                    await w.SaveAsync(context, _server, _database);
                }
                catch (Exception ex)
                {
                    _logger.LogStatus("An error occured while writing outputs", StatusType.Error);
                    _logger.LogStatus(ex.ToString());
                    iserror = true;
                }

                sw.Stop();

                if (iserror)
                {
                    _logger.LogStatus("Output saved with errors", sw.Elapsed, StatusType.Error);
                }
                else
                {
                    _logger.LogStatus("Output saved", sw.Elapsed);
                }
            }
        }
    }
}
