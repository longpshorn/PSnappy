namespace PSnappy
{
    public enum StatusType
    {
        Start,
        Information,
        Warning,
        Error,
        Complete,
    }

    public static class StatusTypeExtensions
    {
        public static string GetLabel(this StatusType type)
        {
            switch (type)
            {
                case StatusType.Start: return "START";
                case StatusType.Complete: return "COMPLETE";
                case StatusType.Warning: return "WARN";
                case StatusType.Error: return "ERROR";
                case StatusType.Information:
                default: return "INFO";
            }
        }

        public static string GetHistoryType(this StatusType type)
        {
            switch (type)
            {
                case StatusType.Start: return "PSnappy Exection Started";
                case StatusType.Complete: return "PSnappy Execution Complete";
                case StatusType.Warning: return "PSnappy Warning";
                case StatusType.Error: return "PSnappy Error";
                case StatusType.Information:
                default: return "PSnappy Information";
            }
        }
    }
}
