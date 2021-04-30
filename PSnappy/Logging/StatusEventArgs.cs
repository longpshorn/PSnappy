using System;

namespace PSnappy
{
    public static class DateTimeHelpers
    {
        public static DateTime GetApplicationStandardTime()
        {
            return DateTime.UtcNow.AddHours(DateTime.Now.IsDaylightSavingTime() ? -5 : -6);
        }
    }

    [Serializable]
    public class StatusEventArgs : EventArgs
    {
        public StatusType StatusType { get; private set; }
        public string Server { get; private set; }
        public string Database { get; private set; }
        public DateTime Timestamp { get; private set; }
        public TimeSpan Elapsed { get; private set; }
        public string Status { get; private set; }

        public double TotalSeconds { get { return Math.Round(Elapsed.TotalSeconds, 2); } }

        public StatusEventArgs(StatusType statustype, string server, string database, string status, TimeSpan elapsed)
        {
            StatusType = statustype;
            Server = server;
            Database = database;
            Status = status;
            Timestamp = DateTimeHelpers.GetApplicationStandardTime();
            Elapsed = elapsed;
        }

        public StatusEventArgs(StatusType statustype, string server, string database, string status)
        {
            StatusType = statustype;
            Server = server;
            Database = database;
            Status = status;
            Timestamp = DateTimeHelpers.GetApplicationStandardTime();
            Elapsed = new TimeSpan(0);
        }
    }
}
