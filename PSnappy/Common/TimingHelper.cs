using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PSnappy
{
    public interface ITimingHelper
    {
        Stopwatch Start();
        Stopwatch Finish(Stopwatch sw, string message, StatusType t = StatusType.Information);
        TimeSpan TimeThis(Action a);
        (TimeSpan elapsed, T result) TimeThis<T>(Func<T> func);
        Task<TimeSpan> TimeThisAsync(Func<Task> function);
        void TimeThis(string message, Action a);
        T TimeThis<T>(string message, Func<T> function);
        Task TimeThisAsync(string message, Func<Task> function, StatusType t = StatusType.Information);
        Task<(TimeSpan elapsed, T result)> TimeThisAsync<T>(Func<Task<T>> function);
    }

    public class TimingHelper : ITimingHelper
    {
        private readonly IStatusLogger _logger;

        public TimingHelper(IStatusLogger logger)
        {
            _logger = logger;
        }

        public Stopwatch Start() => Stopwatch.StartNew();

        public Stopwatch Finish(Stopwatch sw, string message, StatusType t = StatusType.Information)
        {
            sw.Stop();
            _logger.LogStatus(message, sw.Elapsed, t);
            return sw;
        }

        public void TimeThis(string message, Action a)
        {
            var elapsed = this.TimeThis(a);

            _logger.LogStatus(message, elapsed);
        }

        public T TimeThis<T>(string message, Func<T> function)
        {
            (var elapsed, var result) = this.TimeThis(function);

            _logger.LogStatus(message, elapsed);

            return result;
        }

        public TimeSpan TimeThis(Action a)
        {
            var sw = this.Start();
            a();
            sw.Stop();
            return sw.Elapsed;
        }

        public (TimeSpan elapsed, T result) TimeThis<T>(Func<T> func)
        {
            var sw = this.Start();
            var result = func();
            sw.Stop();
            return (sw.Elapsed, result);
        }

        public async Task TimeThisAsync(string message, Func<Task> function, StatusType t = StatusType.Information)
        {
            var elapsed = await this.TimeThisAsync(function);

            _logger.LogStatus(message, elapsed, t);
        }

        public async Task<TimeSpan> TimeThisAsync(Func<Task> function)
        {
            var sw = this.Start();
            await function();
            sw.Stop();
            return sw.Elapsed;
        }

        public async Task<(TimeSpan elapsed, T result)> TimeThisAsync<T>(Func<Task<T>> function)
        {
            var sw = this.Start();
            var result = await function();
            sw.Stop();
            return (sw.Elapsed, result);
        }
    }

    public static class StopwatchExtensions
    {
        public static string FormattedElapsedTimeString(this Stopwatch stopwatch)
        {
            var ms = ((double)stopwatch.ElapsedMilliseconds);
            if (ms < 1000)
                return ms.ToString("#,0ms");
            else
                return (ms / 1000).ToString("#0.0s");
        }
    }

    public static class TimeSpanExtensions
    {
        public static string ToFormattedString(this TimeSpan ts) => ts.ToString(@"mm\:ss\.ff");
    }
}
