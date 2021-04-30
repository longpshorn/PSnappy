using System;
using System.Collections.Generic;

namespace PSnappy
{
    public interface IStatusLogger
    {
        IEnumerable<StatusEventArgs> StatusLog { get; }
        void LogStatus(string message, StatusType statusType = StatusType.Information);
        void LogStatus(string message, TimeSpan elapsed, StatusType statustype = StatusType.Information);
        void LogException(Exception ex, string message = null);
        event EventHandler StatusChanged;
        void UpdateProcessId(Guid processId);
        void UpdateProcessName(string name);
    }

    public class StatusLogger : IStatusLogger
    {
        public event EventHandler StatusChanged;

        private readonly List<StatusEventArgs> _status = new List<StatusEventArgs>();

        public IEnumerable<StatusEventArgs> StatusLog { get { return _status; } }

        private readonly object _thislock = new object();
        private readonly ISqlOptions _options;
        private string _processName;
        private Guid _processId;

        public StatusLogger(ISqlOptions options)
        {
            _options = options;
        }

        public void UpdateProcessId(Guid processId) => _processId = processId;
        public void UpdateProcessName(string name) => _processName = name;

        public virtual void LogStatus(string message, StatusType statusType = StatusType.Information) => LogStatus(message, new TimeSpan(0), statusType);

        public virtual void LogStatus(string message, TimeSpan elapsed, StatusType statustype = StatusType.Information)
        {
            var e = new StatusEventArgs(statustype, _options.Server, _options.Database, message, elapsed);
            lock (_thislock)
            {
                _status.Add(e);
                StatusChanged?.Invoke(this, e);
            }

            Console.WriteLine($"{DateTime.Now} - {elapsed.TotalSeconds.ToString("N2").PadRight(10)} - {statustype.ToString().PadRight(11)} - {message}");

            SaveToProcessHistory(e);
        }

        public virtual void LogException(Exception ex, string message = null)
        {
            message = message != null ? message : ex.Message;
            LogStatus(message, StatusType.Error);
            LogStatus(ex.ToString(), StatusType.Error);
        }

        private void SaveToProcessHistory(StatusEventArgs e)
        {
            using (var c = SqlServerHelper.GetConnection(_options.Server, _options.Database))
            {
                c.Open();
                var sproc = SqlServerHelper.GetSPCommand("uspSaveProcessHistoryRecord", new Dictionary<string, object>()
                {
                    { "@processName", _processName ?? "PSnappy" },
                    { "@historyType", e.StatusType.GetHistoryType() },
                    { "@duration", e.Elapsed.TotalSeconds },
                    { "@details", e.Status.Length <= 4000 ? e.Status : e.Status.Substring(0, 4000) },
                    { "@userName", _options.UserName },
                    { "@machineName", Environment.MachineName },
                    { "@processId", _processId },
                });

                sproc.Connection = c;

                sproc.ExecuteNonQuery();
            }
        }
    }
}
