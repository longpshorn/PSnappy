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

        public RunOptions(IRunOptions runOptions)
            : this(runOptions, runOptions.ProcessId)
        {
        }

        public RunOptions(ISqlOptions sqlOptions, Guid processId)
            : base(sqlOptions.ConnectionString, sqlOptions.UserName)
        {
            this.ProcessId = processId;
        }
    }
}
