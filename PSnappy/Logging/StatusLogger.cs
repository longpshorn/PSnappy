using PSnappy.Common;
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
        private readonly ISqlHelper _sqlHelper;
        private readonly ISqlOptions _options;
        private string _processName;
        private Guid _processId;

        public StatusLogger(
            ISqlHelper sqlHelper,
            ISqlOptions options
        )
        {
            _sqlHelper = sqlHelper;
            _options = options;
        }

        public void UpdateProcessId(Guid processId) => _processId = processId;
        public void UpdateProcessName(string name) => _processName = name;

        public virtual void LogStatus(string message, StatusType statusType = StatusType.Information) => LogStatus(message, new TimeSpan(0), statusType);

        public virtual void LogStatus(string message, TimeSpan elapsed, StatusType statustype = StatusType.Information)
        {
            var (server, database) = _sqlHelper.GetServerAndDatabaseFromConnectionString(_options.ConnectionString);
            var e = new StatusEventArgs(statustype, server, database, message, elapsed);
            lock (_thislock)
            {
                _status.Add(e);
                StatusChanged?.Invoke(this, e);
            }

            Console.WriteLine($"{DateTime.Now} - {elapsed.TotalSeconds,-10:N2} - {statustype,-11} - {message}");

            SaveToProcessHistory(e);
        }

        public virtual void LogException(Exception ex, string message = null)
        {
            message ??= ex.Message;
            LogStatus(message, StatusType.Error);
            LogStatus(ex.ToString(), StatusType.Error);
        }

        private void SaveToProcessHistory(StatusEventArgs e)
        {
            //using var c = _sqlHelper.GetConnection(_options.ConnectionString);
            //c.Open();
            //var sproc = _sqlHelper.GetSPCommand("uspSaveProcessHistoryRecord", new Dictionary<string, object>()
            //    {
            //        { "@processName", _processName ?? "PSnappy" },
            //        { "@historyType", e.StatusType.GetHistoryType() },
            //        { "@duration", e.Elapsed.TotalSeconds },
            //        { "@details", e.Status.Length <= 4000 ? e.Status : e.Status.Substring(0, 4000) },
            //        { "@userName", _options.UserName },
            //        { "@machineName", Environment.MachineName },
            //        { "@processId", _processId },
            //    });

            //sproc.Connection = c;

            //sproc.ExecuteNonQuery();
        }
    }
}
