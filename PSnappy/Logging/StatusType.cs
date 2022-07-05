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
            return type switch
            {
                StatusType.Start => "START",
                StatusType.Complete => "COMPLETE",
                StatusType.Warning => "WARN",
                StatusType.Error => "ERROR",
                _ => "INFO",
            };
        }

        public static string GetHistoryType(this StatusType type)
        {
            return type switch
            {
                StatusType.Start => "PSnappy Exection Started",
                StatusType.Complete => "PSnappy Execution Complete",
                StatusType.Warning => "PSnappy Warning",
                StatusType.Error => "PSnappy Error",
                _ => "PSnappy Information",
            };
        }
    }
}
