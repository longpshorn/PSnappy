using System;

namespace PSnappy
{
    public interface IRunOptions : ISqlOptions
    {
        Guid ProcessId { get; }
    }

    public class RunOptions : SqlOptions, IRunOptions
    {
        public Guid ProcessId { get; }

        public RunOptions(ISqlOptions sqlOptions, Guid processId)
            : base(sqlOptions.Server, sqlOptions.Database, sqlOptions.UserName)
        {
            this.ProcessId = processId;
        }
    }
}
