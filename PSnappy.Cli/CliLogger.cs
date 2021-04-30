using System;
using System.ComponentModel;
using System.Drawing;
using PSnappy;

namespace PSnappy.Cli
{
    public class CLILogger
    {
        public void LogStatus(string message, TimeSpan elapsed, StatusType statustype = StatusType.Information)
        {
            var formatted = $"{DateTime.Now} - {elapsed.TotalSeconds.ToString().PadRight(10)} - {statustype.ToString().PadRight(11)} - {message}";

            if (statustype == StatusType.Error) Console.Error.WriteLine(formatted, Color.Red);
            else if (statustype == StatusType.Warning) Console.WriteLine(new WarningException(formatted).ToString().Replace("System.ComponentModel.WarningException: ", ""), Color.Yellow);
            else Console.WriteLine(formatted);
        }
    }
}
